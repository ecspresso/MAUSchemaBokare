using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace Kronox;

public class RoomExtractor {
    public static Dictionary<string, List<int>> ExtractAvailableRooms(string html) {
        HtmlDocument doc = new();
        doc.LoadHtml(html);

        HtmlNodeCollection? ledigaRum = doc.DocumentNode.SelectNodes("//td[contains(@class, 'grupprum-ledig')]/a");
        var namnOchSiffror = new Dictionary<string, List<int>>();
        // const string pattern = @"boka\('(NI:[^']+)','RESURSER_LOKALER','(\d+)'";
        const string pattern = @"boka\('(OR:[^']+)','RESURSER_LOKALER','(\d+)'";
        foreach (HtmlNode? link in ledigaRum) {
            string onclick = link.GetAttributeValue("onclick", "");
            Match match = Regex.Match(onclick, pattern);
            string ni = match.Groups[1].Value; // NI:..
            int number = Convert.ToInt32(match.Groups[2].Value); // 4

            // Console.WriteLine(ni + " " + number);
            LäggTillData(namnOchSiffror, ni, number);
        }


        return namnOchSiffror;
    }

    private static void LäggTillData(Dictionary<string, List<int>> dictionary, string rum, int siffra) {
        if(!dictionary.ContainsKey(rum)) {
            dictionary[rum] = new List<int>();
        }

        dictionary[rum].Add(siffra);
    }
}