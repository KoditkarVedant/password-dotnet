using Password.Core;
using Xunit;

namespace Password.UnitTests;

public class PasswordGeneratorTests
{
    private readonly IPasswordGenerator _passwordGenerator;

    public PasswordGeneratorTests()
    {
        _passwordGenerator = new PasswordGenerator();
    }

    [Theory]
    [InlineData(5)]
    [InlineData(10)]
    [InlineData(20)]
    public void Should_Return_Password_Of_Requested_Length(int length)
    {
        var password = _passwordGenerator.Generate(new PasswordOptions
        {
            Length = length
        });

        Assert.Equal(length, password.Length);
    }

    [Fact]
    public void Should_Return_Password_With_LowerLetters()
    {
        const int length = 10;

        var password = _passwordGenerator.Generate(new PasswordOptions
        {
            Length = length,
            IncludeLowerCaseLetters = true
        });

        Assert.Equal(length, password.Length);
        var chars = password.ToCharArray();
        Assert.DoesNotContain(chars, Char.IsEmpty);
        Assert.Contains(password.ToCharArray(), char.IsLower);
    }
    
    [Fact]
    public void Should_Return_Password_With_UpperLetters()
    {
        const int length = 10;

        var password = _passwordGenerator.Generate(new PasswordOptions
        {
            Length = length,
            IncludeUpperCaseLetters = true
        });

        Assert.Equal(length, password.Length);
        var chars = password.ToCharArray();
        Assert.DoesNotContain(chars, Char.IsEmpty);
        Assert.Contains(password.ToCharArray(), char.IsUpper);
    }
    
    [Fact]
    public void Should_Return_Password_With_MixedLetters()
    {
        const int length = 10;

        var password = _passwordGenerator.Generate(new PasswordOptions
        {
            Length = length,
            IncludeLowerCaseLetters = true,
            IncludeUpperCaseLetters = true
        });

        Assert.Equal(length, password.Length);
        var chars = password.ToCharArray();
        Assert.DoesNotContain(chars, Char.IsEmpty);
        Assert.Contains(chars, char.IsUpper);
        Assert.Contains(chars, char.IsLower);
    }

    public static class Char
    {
        public static bool IsEmpty(char c) => c == char.MinValue;
    }
}