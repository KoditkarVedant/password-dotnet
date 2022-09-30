using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Password.UnitTests")]

namespace Password.Core;

public interface IRandomCharacterProvider
{
    char GetRandomCharacter();
}

internal abstract class GenericRandomCharacterProvider : IRandomCharacterProvider
{
    private readonly IRandomNumberGenerator _randomNumberGenerator;

    protected GenericRandomCharacterProvider(string characterSet, IRandomNumberGenerator randomNumberGenerator)
    {
        _randomNumberGenerator = randomNumberGenerator;
        CharacterSet = characterSet;
    }

    public string CharacterSet { get; }
    public char GetRandomCharacter() => CharacterSet[_randomNumberGenerator.Next(0, CharacterSet.Length)];
}

internal class RandomUpperCaseLetterProvider : GenericRandomCharacterProvider
{
    public RandomUpperCaseLetterProvider(IRandomNumberGenerator randomNumberGenerator)
        : base(Constants.UpperLetters, randomNumberGenerator)
    {
    }
}

internal class RandomLowerCaseLetterProvider : GenericRandomCharacterProvider
{
    public RandomLowerCaseLetterProvider(IRandomNumberGenerator randomNumberGenerator)
        : base(Constants.LowerLetters, randomNumberGenerator)
    {
    }
}

internal class RandomDigitProvider : GenericRandomCharacterProvider
{
    public RandomDigitProvider(IRandomNumberGenerator randomNumberGenerator)
        : base(Constants.Digits, randomNumberGenerator)
    {
    }
}

internal class RandomSymbolProvider : GenericRandomCharacterProvider
{
    public RandomSymbolProvider(IRandomNumberGenerator randomNumberGenerator)
        : base(Constants.Symbols, randomNumberGenerator)
    {
    }
}
