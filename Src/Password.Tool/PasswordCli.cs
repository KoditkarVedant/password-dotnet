using System.CommandLine;

namespace Password.Tool;

public class PasswordCli : RootCommand
{
    public PasswordCli() : base("Password command-line tool")
    {
        AddCommand(new GenerateCommand());
    }
}
