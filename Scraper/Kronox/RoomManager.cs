namespace Scraper;

public class RoomManager {
    private static readonly List<string> ValidRoomNames = [
        "NI:A0301",
        "NI:A0322",
        "NI:A0401",
        "NI:A0422",
        "NI:A0515",
        "NI:B0303",
        "NI:B0305",
        "NI:B0321",
        "NI:C0208",
        "NI:C0209",
        "NI:C0210",
        "NI:C0301",
        "NI:C0305",
        "NI:C0306",
        "NI:C0309",
        "NI:C0312",
        "NI:C0325",
        "NI:C0401"
    ];

    public static List<string> GetValidRoomNames() {
        // Return a new copy of the list to preserve encapsulation
        return [..ValidRoomNames];
    }

    public static Dictionary<string, Room> GetValidRoomNamesDict() {
        return GetValidRoomNames().ToDictionary(name => name, name => new Room(name));
    }

    public static bool IsValidRoomName(string roomName) {
        return ValidRoomNames.Contains(roomName);
    }
}