using System.Text;
using HtmlAgilityPack;
using Scraper.CLI.OptionGroups;

namespace Scraper;

public class RoomAvailabilityService {
    private const string FileName = ".available_rooms";
    private readonly string _directory;
    private readonly IEnumerable<string> _priority;
    private readonly Dictionary<string, Room> _rooms;

    public RoomAvailabilityService(ExtractOptions options) {
        _rooms = RoomManager.GetValidRoomNamesDict();
        _directory = options.Directory;
        _priority = options.priority;
    }

    public async Task FetchAndSaveAvailableRooms() {
        string websiteUrl = BuildRoomAvailabilityUrl();
        await FetchAndProcessRoomDataFromWeb(websiteUrl, _rooms);
        await SaveAvailableRooms(_rooms);
    }


    private string BuildRoomAvailabilityUrl() {
        StringBuilder names = new();
        foreach(string name in RoomManager.GetValidRoomNames()) {
            names.Append("l.").Append(name.Replace(":", "%3A")).Append("%2C");
        }

        string date = DateTime.Today.AddDays(2).ToString("yyyy-MM-dd");
        return $"https://schema.mau.se/setup/jsp/Schema.jsp?startDatum={date}&intervallTyp=d&intervallAntal=1&sokMedAND=false&sprak=SV&resurser={names}";
    }

    private async Task FetchAndProcessRoomDataFromWeb(string websiteUrl, Dictionary<string, Room> rooms) {
        WebHandler webHandler = new();
        string htmlcontent = await webHandler.FetchWebsiteData(websiteUrl);
        webHandler.Dispose();
        Scraper scraper = new(rooms);
        HtmlNodeCollection? collection = scraper.ExtractRows(htmlcontent);
        if(collection is not null) {
            scraper.ExtractSchedule(collection);
        }
    }

    private IEnumerable<string> FilterAndSortRooms(Dictionary<string, Room> rooms) {
        return rooms
            .Where(room => !room.Value.Slots[1] && !room.Value.Slots[2])
            .OrderBy(room => _priority.Contains(room.Value.Name) ? 0 : 1)
            .ThenBy(room => Array.IndexOf(_priority.ToArray(), room.Value.Name))
            .Select(room => room.Value.Name.Replace(":", "%3A"));
    }

    private async Task SaveAvailableRooms(Dictionary<string, Room> rooms) {
        var filteredRooms = FilterAndSortRooms(rooms);

        if(File.Exists(FileName)) {
            File.Delete(FileName);
            Console.WriteLine("Filen har tagits bort.");
        }

        string fullPath = Path.Combine(_directory, FileName);

        await using StreamWriter writer = new(fullPath);

        foreach(string room in filteredRooms) {
            await writer.WriteLineAsync(room);
        }
    }
}