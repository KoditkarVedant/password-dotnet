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
        var password = _passwordGenerator.Generate(new PasswordOptions{
            Length = length
        });

        Assert.Equal(length, password.Length);
    }
}
