using HtmlAgilityPack;

namespace Scraper;

public class Scraper(Dictionary<string, Room> rooms) {
    private int _cellNameLocation;
    private int _cellTimeLocation;


    public HtmlNodeCollection? ExtractRows(string html) {
        HtmlDocument doc = new();
        doc.LoadHtml(html);

        if(doc.DocumentNode.InnerHtml.Contains("Inga scheman kunde hittas som matchar din sökning:")) {
            return null;
        }

        const string xpathHeader = "//table[@class='schemaTabell']//tr[@class='heading']/th";

        _cellNameLocation = ExtractCellLocation(doc, xpathHeader, "Start-Slut");
        _cellTimeLocation = ExtractCellLocation(doc, xpathHeader, "Lokal");

        string xpath = "//table[contains(@class, 'schemaTabell')]//tr[contains(@class, 'data-white') or contains(@class, 'data-grey')]";
        return doc.DocumentNode.SelectNodes(xpath);
    }

    private int ExtractCellLocation(HtmlDocument doc, string xpath, string matchAgainst) {
        HtmlNodeCollection? nodes = doc.DocumentNode.SelectNodes(xpath);
        int location = 0;

        foreach(HtmlNode node in nodes) {
            location++;
            if(node.InnerHtml.Contains(matchAgainst)) {
                break;
            }
        }

        return location;
    }


    public void ExtractSchedule(HtmlNodeCollection collection) {
        foreach(HtmlNode? node in collection) {
            HtmlNodeCollection childNodes = node.ChildNodes;

            string time = childNodes[_cellNameLocation].FirstChild.InnerHtml.Replace("&nbsp;", "");

            foreach(HtmlNode room in childNodes[_cellTimeLocation].ChildNodes) {
                string name = room.InnerHtml;
                if(RoomManager.IsValidRoomName(name)) {
                    TrackBookings(name, time);
                }
            }
        }
    }

    private void TrackBookings(string name, string time) {
        rooms[name].Track(time);
    }
}