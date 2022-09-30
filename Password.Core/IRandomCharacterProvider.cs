using System.Runtime.CompilerServices;

[assembly:InternalsVisibleTo("Password.UnitTests")]
namespace Password.Core;

public interface IRandomCharacterProvider
{
    char GetRandomCharacter();
}

internal abstract class GenericRandomCharacterProvider : IRandomCharacterProvider
{
    protected GenericRandomCharacterProvider(string characterSet)
    {
        CharacterSet = characterSet;
    }

    public string CharacterSet { get; }
    public char GetRandomCharacter() => CharacterSet[Random.Shared.Next(0, CharacterSet.Length)];
}

internal class RandomUpperCaseLetterProvider : GenericRandomCharacterProvider
{
    public RandomUpperCaseLetterProvider() : base(Constants.UpperLetters)
    {
    }
}

internal class RandomLowerCaseLetterProvider : GenericRandomCharacterProvider
{
    public RandomLowerCaseLetterProvider() : base(Constants.LowerLetters)
    {
    }
}

internal class RandomDigitProvider : GenericRandomCharacterProvider
{
    public RandomDigitProvider() : base(Constants.Digits)
    {
    }
}

internal class RandomSymbolProvider : GenericRandomCharacterProvider
{
    public RandomSymbolProvider() : base(Constants.Symbols)
    {
    }
}
