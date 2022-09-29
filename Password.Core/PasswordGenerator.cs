namespace Password.Core;

public class PasswordOptions
{
    public int Length { get; set; }
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
        return new string(chars);
    }
}
