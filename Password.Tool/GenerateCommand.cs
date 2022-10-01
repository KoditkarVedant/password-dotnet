using System.CommandLine;
using Password.Core;

namespace Password.Tool;

public class GenerateCommand : Command
{
    private readonly Option<int> _lengthOption = new Option<int>(
        aliases: new[] { "--length", "-l" },
        getDefaultValue: () => 15,
        description: "Length of a password"
    );

    private readonly Option<bool> _includeLowerCase = new Option<bool>(
        aliases: new[] { "--include-lower-case", "-ilc" },
        getDefaultValue: () => true,
        description: "Include lowercase letters"
    );

    private readonly Option<bool> _includeUpperCase = new Option<bool>(
        aliases: new[] { "--include-upper-case", "-ipc" },
        getDefaultValue: () => true,
        description: "Include Uppercase letters"
    );

    private readonly Option<bool> _includeDigit = new Option<bool>(
        aliases: new[] { "--include-digit", "-id" },
        getDefaultValue: () => true,
        description: "Include digits"
    );

    private readonly Option<bool> _includeSymbol = new Option<bool>(
        aliases: new[] { "--include-symbol", "-is" },
        getDefaultValue: () => true,
        description: "Include symbols"
    );

    public GenerateCommand() : base("generate", "Generate a password")
    {
        AddOption(_lengthOption);
        AddOption(_includeLowerCase);
        AddOption(_includeUpperCase);
        AddOption(_includeDigit);
        AddOption(_includeSymbol);

        this.SetHandler(
            GenerateHandler,
            new PasswordOptionsBinder(
                _lengthOption,
                _includeLowerCase,
                _includeUpperCase,
                _includeDigit,
                _includeSymbol
            )
        );
    }

    private static void GenerateHandler(PasswordOptions options)
    {
        var passwordGenerator = PasswordGeneratorFactory.GetGenerator();
        Console.WriteLine(passwordGenerator.Generate(options));
    }
}
