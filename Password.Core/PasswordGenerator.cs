using System.Text;

namespace Password.Core;

public interface IPasswordGenerator
{
    string Generate(PasswordOptions options);
}

public class PasswordGenerator : IPasswordGenerator
{
    private readonly ICharDistribution _charDistribution;
    private readonly Dictionary<PasswordCharType, IRandomCharacterProvider> _randomCharacterProviders;
    private readonly IRandomNumberGenerator _randomNumberGenerator;

    private static readonly IReadOnlyList<PasswordCharType> DefaultEnabledCharTypes = new List<PasswordCharType>()
    {
        PasswordCharType.LowerCase
    };

    public PasswordGenerator(
        ICharDistribution charDistribution,
        Dictionary<PasswordCharType, IRandomCharacterProvider> randomCharacterProviders,
        IRandomNumberGenerator randomNumberGenerator)
    {
        _charDistribution = charDistribution;
        _randomCharacterProviders = randomCharacterProviders;
        _randomNumberGenerator = randomNumberGenerator;
    }

    public string Generate(PasswordOptions options)
    {
        var passwordCharQueue = new PriorityQueue<char, int>(options.Length);

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
            options.IncludeLowerCaseLetters = true;
            enabledTypes = DefaultEnabledCharTypes;
        }

        var distribution = _charDistribution.Distribute(enabledTypes, options.Length);

        if (options.IncludeLowerCaseLetters)
        {
            AddRandomCharacters(PasswordCharType.LowerCase, distribution, passwordCharQueue);
        }

        if (options.IncludeUpperCaseLetters)
        {
            AddRandomCharacters(PasswordCharType.UpperCase, distribution, passwordCharQueue);
        }

        if (options.IncludeDigits)
        {
            AddRandomCharacters(PasswordCharType.Digit, distribution, passwordCharQueue);
        }

        if (options.IncludeSymbols)
        {
            AddRandomCharacters(PasswordCharType.Symbol, distribution, passwordCharQueue);
        }

        return BuildPasswordString(passwordCharQueue);
    }

    private static string BuildPasswordString(PriorityQueue<char, int> passwordCharQueue)
    {
        var passwordBuilder = new StringBuilder();
        while (passwordCharQueue.TryDequeue(out var element, out _))
        {
            passwordBuilder.Append(element);
        }

        return passwordBuilder.ToString();
    }

    private void AddRandomCharacters(
        PasswordCharType charType,
        IReadOnlyDictionary<PasswordCharType, int> distribution,
        PriorityQueue<char, int> chars)
    {
        for (var i = 0; i < distribution[charType]; i++)
        {
            var provider = _randomCharacterProviders[charType];
            var randomChar = provider.GetRandomCharacter();

            var randomInsertPosition = _randomNumberGenerator.Next(0, chars.Count + 1);

            chars.Enqueue(randomChar, randomInsertPosition);
        }
    }
}
