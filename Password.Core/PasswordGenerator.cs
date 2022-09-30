namespace Password.Core;

public interface IPasswordGenerator
{
    string Generate(PasswordOptions options);
}

public class PasswordGenerator : IPasswordGenerator
{
    private readonly ICharDistribution _charDistribution;
    private readonly Dictionary<PasswordCharType, IRandomCharacterProvider> _randomCharacterProviders;

    private readonly IReadOnlyList<PasswordCharType> DefaultEnabledCharTypes = new List<PasswordCharType>()
    {
        PasswordCharType.LowerCase
    };

    public PasswordGenerator(
        ICharDistribution charDistribution,
        Dictionary<PasswordCharType, IRandomCharacterProvider> randomCharacterProviders)
    {
        _charDistribution = charDistribution;
        _randomCharacterProviders = randomCharacterProviders;
    }

    public string Generate(PasswordOptions options)
    {
        char[] chars = new char[options.Length];

        var charTypes = new Dictionary<PasswordCharType, bool>()
        {
            { PasswordCharType.LowerCase, options.IncludeLowerCaseLetters },
            { PasswordCharType.UpperCase, options.IncludeUpperCaseLetters },
            { PasswordCharType.Digit, options.IncludeDigits },
            { PasswordCharType.Symbol, options.IncludeSymbols }
        };

        IReadOnlyList<PasswordCharType> enabledTypes = charTypes
            .Where(x => x.Value)
            .Select(x => x.Key)
            .ToList();


        if (enabledTypes.Count <= 0)
        {
            enabledTypes = DefaultEnabledCharTypes;
        }

        var distribution = _charDistribution.Distribute(enabledTypes, options.Length);

        var currentCharIndex = 0;

        if (options.IncludeLowerCaseLetters)
        {
            AddRandomCharacters(PasswordCharType.LowerCase, distribution, chars, ref currentCharIndex);
        }

        if (options.IncludeUpperCaseLetters)
        {
            AddRandomCharacters(PasswordCharType.UpperCase, distribution, chars, ref currentCharIndex);
        }

        if (options.IncludeDigits)
        {
            AddRandomCharacters(PasswordCharType.Digit, distribution, chars, ref currentCharIndex);
        }

        if (options.IncludeSymbols)
        {
            AddRandomCharacters(PasswordCharType.Symbol, distribution, chars, ref currentCharIndex);
        }

        return new string(chars);
    }

    private void AddRandomCharacters(
        PasswordCharType charType,
        IReadOnlyDictionary<PasswordCharType, int> distribution,
        IList<char> chars,
        ref int currentCharIndex)
    {
        var provider = _randomCharacterProviders[charType];
        for (var i = 0; i < distribution[charType]; i++)
        {
            chars[currentCharIndex] = provider.GetRandomCharacter();
            currentCharIndex++;
        }
    }
}
