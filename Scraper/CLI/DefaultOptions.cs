using CommandLine;

namespace Scraper.CLI;

public class DefaultOptions {
    private string _directory;

    [Option(
        'd',
        "Directory",
        HelpText = "Katalogen där input filerna finns."
    )]
    // Alla grupper
    public string Directory {
        get => _directory ?? GetDefault();
        set => _directory = value;
    }

    private static string GetDefault() {
        return Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
    }
}