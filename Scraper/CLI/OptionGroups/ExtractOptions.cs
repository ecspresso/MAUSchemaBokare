using CommandLine;

namespace Scraper.CLI.OptionGroups;

[Verb("skrapa", HelpText = "Extrahera möjliga rum att boka.")]
public class ExtractOptions : DefaultOptions {
    private IEnumerable<string> _priority;

    [Option(
        'p',
        "Priority",
        SetName = "Extract",
        Separator = ',',
        HelpText = "Vilket rum att boka först i prioritetsordning, flera rum är tillåtna."
    )]
    public IEnumerable<string> priority {
        get => _priority;
        set {
            if(value.All(RoomManager.IsValidRoomName)) {
                _priority = value;
            } else {
                List<string> invalid = [];
                invalid.AddRange(value.Where(v => !RoomManager.IsValidRoomName(v)));

                throw new ArgumentException($"{'\n'}  Invalid priority values: [{string.Join(", ", invalid)}]. {'\n'}" +
                                            $"  Valid options are {string.Join(", ", RoomManager.GetValidRoomNames())}");
            }
        }
    }
}