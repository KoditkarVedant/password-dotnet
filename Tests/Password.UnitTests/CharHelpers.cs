using Password.Core;

namespace Password.UnitTests;

internal static class CharHelpers
{
    internal static bool IsEmpty(char c) => c == char.MinValue;

    internal static bool IsPasswordSymbol(char c) => Constants.Symbols.Contains(c);
}
