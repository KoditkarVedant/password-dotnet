using Password.Core;

namespace Password.UnitTests;

public static class CharHelpers
{
    public static bool IsEmpty(char c) => c == char.MinValue;

    public static bool IsPasswordSymbol(char c) => Constants.Symbols.Contains(c);
}
