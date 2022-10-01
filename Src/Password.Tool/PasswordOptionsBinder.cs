using System.CommandLine;
using System.CommandLine.Binding;
using Password.Core;

namespace Password.Tool;

public class PasswordOptionsBinder : BinderBase<PasswordOptions>
{
    private readonly Option<int> _lengthOption;
    private readonly Option<bool> _includeLowerCase;
    private readonly Option<bool> _includeUpperCase;
    private readonly Option<bool> _includeDigit;
    private readonly Option<bool> _includeSymbol;

    public PasswordOptionsBinder(
        Option<int> lengthOption,
        Option<bool> includeLowerCase,
        Option<bool> includeUpperCase,
        Option<bool> includeDigit,
        Option<bool> includeSymbol)
    {
        _lengthOption = lengthOption;
        _includeLowerCase = includeLowerCase;
        _includeUpperCase = includeUpperCase;
        _includeDigit = includeDigit;
        _includeSymbol = includeSymbol;
    }

    protected override PasswordOptions GetBoundValue(BindingContext bindingContext)
    {
        return new PasswordOptions()
        {
            Length = bindingContext.ParseResult.GetValueForOption(_lengthOption),
            IncludeLowerCaseLetters = bindingContext.ParseResult.GetValueForOption(_includeLowerCase),
            IncludeUpperCaseLetters = bindingContext.ParseResult.GetValueForOption(_includeUpperCase),
            IncludeDigits = bindingContext.ParseResult.GetValueForOption(_includeDigit),
            IncludeSymbols = bindingContext.ParseResult.GetValueForOption(_includeSymbol),
        };
    }
}
