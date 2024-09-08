using Scraper.CLI.OptionGroups;

namespace Scraper.CLI.Main;

public class RoomBookingService {
    private const string FilePath = ".available_rooms";
    private readonly string _directory;
    private readonly string _password;
    private readonly string _username;


    public RoomBookingService(BookOptions options) {
        _username = options.Username;
        _password = options.Password;
        _directory = options.Directory;
    }

    public async Task BookRooms() {
        WebHandler webHandler = new();
        await webHandler.Login(_username, _password);

        // Öppna filen för läsning
        string path = Path.Join(_directory, FilePath);
        using StreamReader reader = new(path);
        string? room;
        bool bookingSuccessful = false;

        while ((room = reader.ReadLine()) is not null && !bookingSuccessful) {
            string date = DateTime.Today.AddDays(1).ToString("yy-MM-dd");

            bookingSuccessful = await webHandler.Book(room, date);
        }

        webHandler.Dispose();

        if(bookingSuccessful) {
            Console.WriteLine("Bokat rum.");
        } else {
            Console.WriteLine("Inte bokat rum.");
        }
    }
}