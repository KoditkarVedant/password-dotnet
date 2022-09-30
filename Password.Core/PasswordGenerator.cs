namespace Password.Core;

public interface IPasswordGenerator
{
    string Generate(PasswordOptions options);
}

public class PasswordGenerator : IPasswordGenerator
{
    private readonly ICharDistribution _charDistribution;

    public PasswordGenerator(ICharDistribution charDistribution)
    {
        _charDistribution = charDistribution;
    }

    public string Generate(PasswordOptions options)
    {
        char[] chars = new char[options.Length];

        var charTypes = new Dictionary<string, bool>()
        {
            { "LowerCase", options.IncludeLowerCaseLetters },
            { "UpperCase", options.IncludeUpperCaseLetters },
            { "Digits", options.IncludeDigits },
            { "Symbols", options.IncludeSymbols }
        };

        var enabledTypes = charTypes
            .Where(x => x.Value)
            .Select(x => x.Key)
            .ToList();


        if (enabledTypes.Count <= 0)
        {
            enabledTypes = new List<string> { "LowerCase" };
        }

        var distribution = _charDistribution.Distribute(enabledTypes, options.Length);

        var currentCharIndex = 0;

        if (options.IncludeLowerCaseLetters)
        {
            for (var i = 0; i < distribution["LowerCase"]; i++)
            {
                var randomLowerCharacterIndex = Random.Shared.Next(0, Constants.LowerLetters.Length);

                chars[currentCharIndex] = Constants.LowerLetters[randomLowerCharacterIndex];
                currentCharIndex++;
            }
        }

        if (options.IncludeUpperCaseLetters)
        {
            for (var i = 0; i < distribution["UpperCase"]; i++)
            {
                var randomUpperCharacterIndex = Random.Shared.Next(0, Constants.UpperLetters.Length);

                chars[currentCharIndex] = Constants.UpperLetters[randomUpperCharacterIndex];
                currentCharIndex++;
            }
        }

        if (options.IncludeDigits)
        {
            for (var i = 0; i < distribution["Digits"]; i++)
            {
                var randomDigitCharacterIndex = Random.Shared.Next(0, Constants.Digits.Length);

                chars[currentCharIndex] = Constants.Digits[randomDigitCharacterIndex];
                currentCharIndex++;
            }
        }

        if (options.IncludeSymbols)
        {
            for (var i = 0; i < distribution["Symbols"]; i++)
            {
                var randomSymbolCharacterIndex = Random.Shared.Next(0, Constants.Symbols.Length);

                chars[currentCharIndex] = Constants.Symbols[randomSymbolCharacterIndex];
                currentCharIndex++;
            }
        }

        return new string(chars);
    }
}
