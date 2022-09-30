namespace Password.Core;

public sealed class PasswordCharType : Enum<int>
{
    public static readonly PasswordCharType LowerCase = new(2, "LowerCase");
    public static readonly PasswordCharType UpperCase = new(2, "UpperCase");
    public static readonly PasswordCharType Digit = new(2, "Digit");
    public static readonly PasswordCharType Symbol = new(2, "Symbol");

    private PasswordCharType(int id, string name) : base(id, name)
    {
    }
}
