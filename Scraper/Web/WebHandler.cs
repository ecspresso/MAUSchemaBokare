using System.Text;

namespace Scraper;

internal class WebHandler {
    private readonly HttpClient _client = new();
    private HttpResponseMessage? _response;

    public WebHandler() {
        _client.DefaultRequestHeaders.Add("User-Agent",
            "Mozilla/5.0 (Windows NT 10.0; rv:123.0) Gecko/20100101 Firefox/123.0");
    }

    public async Task<string> FetchWebsiteData(string url) {
        HttpResponseMessage response = await _client.GetAsync(url);
        response.EnsureSuccessStatusCode(); // Kastar ett undantag om svarskoden inte indikerar framgång

        return await response.Content.ReadAsStringAsync();
    }

    public async Task<bool> Login(string username, string password) {
        try {
            // Ange URL för inloggningssidan
            const string loginUrl = "https://schema.mau.se/login_do.jsp";

            // Skapa inloggningsdata (användarnamn och lösenord)
            StringContent loginData = new($"username={username}&password={password}", Encoding.UTF8,
                "application/x-www-form-urlencoded");

            // Skicka inloggningsförfrågan med POST-metoden
            _response = await _client.PostAsync(loginUrl, loginData);
            _response.EnsureSuccessStatusCode();

            // Kontrollera om inloggningen lyckades genom att titta på svarskoden
            return _response.IsSuccessStatusCode;
        } catch(Exception ex) {
            Console.WriteLine("Ett fel uppstod vid inloggning: " + ex.Message);
            return false;
        }
    }

    public async Task<bool> Book(string name, string date) {
        string url1 = $"https://schema.mau.se/ajax/ajax_resursbokning.jsp?op=boka&datum={date}&id={name}&typ=RESURSER_LOKALER&intervall=1&moment=kaffe&flik=FLIK-0017";
        string url2 = $"https://schema.mau.se/ajax/ajax_resursbokning.jsp?op=boka&datum={date}&id={name}&typ=RESURSER_LOKALER&intervall=2&moment=kaffe&flik=FLIK-0017";

        // Skapa uppgift för att göra GET-anropen parallellt
        var get1Task = _client.GetStringAsync(url1);
        var get2Task = _client.GetStringAsync(url2);

        // Vänta på att båda GET-anropen ska slutföras
        await Task.WhenAll(get1Task, get2Task);

        if(await get1Task != "OK" || await get1Task != "OK") {
            Console.WriteLine($"GET1: {get1Task.Result}");
            Console.WriteLine($"GET2: {get2Task.Result}");
            return false;
        }

        Console.WriteLine($"Bokat rum {name}");
        return true;
    }

    public void Unbook(string room, string date) { }

    public void Dispose() {
        _client.Dispose();
    }
}