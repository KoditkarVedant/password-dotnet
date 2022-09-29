namespace Password.Core;

public class PasswordOptions
{
    public int Length { get; set; }
    public bool IncludeLowerCaseLetters { get; set; }
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

        if (options.IncludeLowerCaseLetters)
        {
            for (var i = 0; i < options.Length; i++)
            {
                var randomLowerCharacterIndex = Random.Shared.Next(0, Constants.LowerLetters.Length);

                chars[i] = Constants.LowerLetters[randomLowerCharacterIndex];
            }
        }

        return new string(chars);
    }
}
