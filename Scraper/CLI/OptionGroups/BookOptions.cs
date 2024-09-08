using CommandLine;

namespace Scraper.CLI.OptionGroups;

[Verb("boka", HelpText = "Boka ett rum.")]
public class BookOptions : DefaultOptions {
    [Option(
        'u',
        "username",
        SetName = "Book",
        Required = true,
        HelpText = "Användarnamn till användare som ska boka i Kronox."
    )]
    public string Username { get; set; }

    [Option('p',
        "password",
        SetName = "Book",
        Required = true,
        HelpText = "Lösenord till användare som ska boka i Kronox."
    )]
    public string Password { get; set; }
}