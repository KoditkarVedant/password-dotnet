namespace Password.Core;

public class PasswordOptions : ICloneable
{
    public int Length { get; set; }
    public bool IncludeLowerCaseLetters { get; set; }
    public bool IncludeUpperCaseLetters { get; set; }
    public bool IncludeDigits { get; set; }
    public bool IncludeSymbols { get; set; }

    public object Clone()
    {
        return new PasswordOptions
        {
            Length = Length,
            IncludeDigits = IncludeDigits,
            IncludeSymbols = IncludeSymbols,
            IncludeLowerCaseLetters = IncludeLowerCaseLetters,
            IncludeUpperCaseLetters = IncludeUpperCaseLetters
        };
    }
}
