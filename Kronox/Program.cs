using CommandLine;

namespace Kronox;

internal static class Program {
    private static string Date { get; } = DateTime.Today.AddDays(1).ToString("yy-MM-dd");

    private static void Main(string[] args) {
        Parser.Default.ParseArguments<Parameters>(args).WithParsed(o => {
            Run(o.Username, o.Password).Wait();
        });
    }

    private static async Task Run(string username, string password) {
        WebHandler webHandler = new();

        // Logga in
        Console.WriteLine("logga in");
        bool loginResult = await webHandler.Login(username, password);
        Console.WriteLine("logga in klart");
        if(loginResult) {
            Console.WriteLine("Inloggning lyckades.");
        }
        else {
            Console.WriteLine("Inloggning misslyckades.");
        }


        // Anropa webbplatsen
        string websiteUrl = $"https://schema.mau.se/ajax/ajax_resursbokning.jsp?op=hamtaBokningar&datum={Date}&flik=FLIK-0017";
        string websiteData = await webHandler.FetchWebsiteData(websiteUrl);
        var rum = RoomExtractor.ExtractAvailableRooms(websiteData);

        foreach (var kvp in rum) {
            string namn = kvp.Key;
            var siffror = kvp.Value;

            // Om namnet har siffran 1 och siffran 2
            if(siffror.Contains(1) && siffror.Contains(2)) {
                await webHandler.Book(namn, Date);
            }
        }


        webHandler.Dispose();
    }
}