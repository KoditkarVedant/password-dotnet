namespace Password.Core;

internal static class PasswordOptionsMerger
{
    private static readonly PasswordOptions DefaultPasswordOptions = new()
    {
        Length = 15,
        IncludeLowerCaseLetters = true,
        IncludeDigits = true,
        IncludeSymbols = true,
        IncludeUpperCaseLetters = true
    };

    internal static PasswordOptions Merge(PasswordOptions options)
    {
        var mergedOptions = (PasswordOptions) DefaultPasswordOptions.Clone();

        if (options.Length > 0)
        {
            mergedOptions.Length = options.Length;
        }

        if (options.IncludeSymbols
            || options.IncludeDigits
            || options.IncludeUpperCaseLetters
            || options.IncludeLowerCaseLetters)
        {
            mergedOptions.IncludeLowerCaseLetters = options.IncludeLowerCaseLetters;
            mergedOptions.IncludeUpperCaseLetters = options.IncludeUpperCaseLetters;
            mergedOptions.IncludeDigits = options.IncludeDigits;
            mergedOptions.IncludeSymbols = options.IncludeSymbols;
        }
        else
        {
            mergedOptions.IncludeLowerCaseLetters = true;
            mergedOptions.IncludeUpperCaseLetters = DefaultPasswordOptions.IncludeUpperCaseLetters;
            mergedOptions.IncludeDigits = DefaultPasswordOptions.IncludeDigits;
            mergedOptions.IncludeSymbols = DefaultPasswordOptions.IncludeSymbols;
        }

        return mergedOptions;
    }
}
