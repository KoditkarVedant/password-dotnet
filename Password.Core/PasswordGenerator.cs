namespace Password.Core;

public class PasswordOptions
{
    public int Length { get; set; }
    public bool IncludeLowerCaseLetters { get; set; }
    public bool IncludeUpperCaseLetters { get; set; }
}

public interface IPasswordGenerator
{
    string Generate(PasswordOptions options);
}

public class PasswordGenerator : IPasswordGenerator
{
    public string Generate(PasswordOptions options)
    {
        char[] chars = new char[options.Length];

        var totalTypesOfChars = new[] { options.IncludeLowerCaseLetters, options.IncludeUpperCaseLetters };
        var enabledTypes = totalTypesOfChars.Count(x => x);
        if (enabledTypes <= 0)
        {
            enabledTypes = 1;
        }

        var charPerType = options.Length / enabledTypes;

        int currentCharIndex = 0;
        
        if (options.IncludeLowerCaseLetters)
        {
            for (var i = 0; i < charPerType; i++)
            {
                var randomLowerCharacterIndex = Random.Shared.Next(0, Constants.LowerLetters.Length);

                chars[currentCharIndex] = Constants.LowerLetters[randomLowerCharacterIndex];
                currentCharIndex++;
            }
        }

        if (options.IncludeUpperCaseLetters)
        {
            for (var i = 0; i < charPerType; i++)
            {
                var randomUpperCharacterIndex = Random.Shared.Next(0, Constants.UpperLetters.Length);

                chars[currentCharIndex] = Constants.UpperLetters[randomUpperCharacterIndex];
                currentCharIndex++;
            }
        }

        return new string(chars);
    }
}
