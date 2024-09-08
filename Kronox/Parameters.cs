using CommandLine;

namespace Kronox;

public class Parameters {
    [Option('u', "username", Required = true, HelpText = "Användarnamn till användare som ska boka i Kronox.")]
    public required string Username { get; set; }

    [Option('p', "password", Required = true, HelpText = "Lösenord till användare som ska boka i Kronox.")]
    public required string Password { get; set; }
}