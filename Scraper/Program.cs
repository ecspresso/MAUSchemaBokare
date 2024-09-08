using CommandLine;
using Scraper.CLI.Main;
using Scraper.CLI.OptionGroups;

namespace Scraper;

internal class Program {
    public static void Main(string[] args) {
        var result = Parser.Default.ParseArguments<BookOptions, ExtractOptions>(args);
        result.WithParsed(options => {
            switch (options) {
                case BookOptions bookOptions:
                    RoomBookingService bookingService = new(bookOptions);
                    bookingService.BookRooms().Wait();
                    break;
                case ExtractOptions extractOptions:
                    RoomAvailabilityService availabilityService = new(extractOptions);
                    availabilityService.FetchAndSaveAvailableRooms().Wait();
                    break;
            }
        });
    }
}