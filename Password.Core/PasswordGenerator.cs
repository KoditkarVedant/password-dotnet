using System.Text;

namespace Password.Core;

public interface IPasswordGenerator
{
    string Generate(PasswordOptions options);
}

internal class PasswordGenerator : IPasswordGenerator
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

    public string Generate(PasswordOptions options) => InternalGenerate(PasswordOptionsMerger.Merge(options));

    private string InternalGenerate(PasswordOptions options)
    {
        var enabledPasswordCharTypes = GetEnabledPasswordCharTypes(options);
        var passwordCharDistribution = _charDistribution.Distribute(enabledPasswordCharTypes, options.Length);

        var passwordCharQueue = new PriorityQueue<char, int>(options.Length);

        foreach (var (passwordCharType, totalCharsToAdd) in passwordCharDistribution)
        {
            var provider = _randomCharacterProviders[passwordCharType];
            for (var i = 0; i < totalCharsToAdd; i++)
            {
                var randomChar = provider.GetRandomCharacter();
                var randomInsertPosition = _randomNumberGenerator.Next(0, passwordCharQueue.Count + 1);
                passwordCharQueue.Enqueue(randomChar, randomInsertPosition);
            }
        }

        return BuildPasswordString(passwordCharQueue);
    }

    private static string BuildPasswordString(PriorityQueue<char, int> passwordCharQueue)
    {
        var passwordBuilder = new StringBuilder(passwordCharQueue.Count);
        while (passwordCharQueue.TryDequeue(out var element, out _))
        {
            passwordBuilder.Append(element);
        }

        return passwordBuilder.ToString();
    }

    private static IEnumerable<PasswordCharType> GetEnabledPasswordCharTypes(PasswordOptions options)
    {
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

        return enabledTypes;
    }
}
