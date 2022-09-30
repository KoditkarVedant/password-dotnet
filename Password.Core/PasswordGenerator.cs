namespace Password.Core;

public interface IPasswordGenerator
{
    string Generate(PasswordOptions options);
}

public class PasswordGenerator : IPasswordGenerator
{
    private readonly ICharDistribution _charDistribution;

    private readonly Dictionary<string, IRandomCharacterProvider> _randomCharacterProviders;

    public PasswordGenerator(
        ICharDistribution charDistribution,
        Dictionary<string, IRandomCharacterProvider> randomCharacterProviders)
    {
        _charDistribution = charDistribution;
        _randomCharacterProviders = randomCharacterProviders;
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
            AddRandomCharacters("LowerCase", distribution, chars, ref currentCharIndex);
        }

        if (options.IncludeUpperCaseLetters)
        {
            AddRandomCharacters("UpperCase", distribution, chars, ref currentCharIndex);
        }

        if (options.IncludeDigits)
        {
            AddRandomCharacters("Digits", distribution, chars, ref currentCharIndex);
        }

        if (options.IncludeSymbols)
        {
            AddRandomCharacters("Symbols", distribution, chars, ref currentCharIndex);
        }

        return new string(chars);
    }

    private void AddRandomCharacters(
        string charType,
        IReadOnlyDictionary<string, int> distribution,
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
