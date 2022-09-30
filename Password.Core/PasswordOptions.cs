namespace Password.Core;

public class PasswordOptions
{
    public int Length { get; set; }
    public bool IncludeLowerCaseLetters { get; set; }
    public bool IncludeUpperCaseLetters { get; set; }
    public bool IncludeDigits { get; set; }
}
