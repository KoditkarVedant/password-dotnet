using System.Collections.Generic;
using Moq;
using Password.Core;
using Xunit;

namespace Password.UnitTests;

public class PasswordGeneratorTests
{
    private readonly IPasswordGenerator _passwordGenerator;
    private readonly Mock<ICharDistribution> _charDistributionMock;

    public PasswordGeneratorTests()
    {
        _charDistributionMock = new Mock<ICharDistribution>();
        _passwordGenerator = new PasswordGenerator(_charDistributionMock.Object);
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
        // Arrange
        const int length = 10;
        _charDistributionMock
            .Setup(x => x.Distribute(It.IsAny<IEnumerable<string>>(), length))
            .Returns(new Dictionary<string, int>()
            {
                { "LowerCase", 10 }
            });

        // Act
        var password = _passwordGenerator.Generate(new PasswordOptions
        {
            Length = length,
            IncludeLowerCaseLetters = true
        });

        // Assert
        Assert.Equal(length, password.Length);
        var chars = password.ToCharArray();
        Assert.DoesNotContain(chars, CharHelpers.IsEmpty);
        Assert.Contains(password.ToCharArray(), char.IsLower);
    }

    [Fact]
    public void Should_Return_Password_With_UpperLetters()
    {
        // Arrange
        const int length = 10;
        _charDistributionMock
            .Setup(x => x.Distribute(It.IsAny<IEnumerable<string>>(), length))
            .Returns(new Dictionary<string, int>()
            {
                { "UpperCase", 10 }
            });

        // Act
        var password = _passwordGenerator.Generate(new PasswordOptions
        {
            Length = length,
            IncludeUpperCaseLetters = true
        });

        // Assert
        Assert.Equal(length, password.Length);
        var chars = password.ToCharArray();
        Assert.DoesNotContain(chars, CharHelpers.IsEmpty);
        Assert.Contains(password.ToCharArray(), char.IsUpper);
    }

    [Fact]
    public void Should_Return_Password_With_MixedLetters()
    {
        // Arrange
        const int length = 10;
        _charDistributionMock
            .Setup(x => x.Distribute(It.IsAny<IEnumerable<string>>(), length))
            .Returns(new Dictionary<string, int>()
            {
                { "LowerCase", 5 },
                { "UpperCase", 5 }
            });

        // Act
        var password = _passwordGenerator.Generate(new PasswordOptions
        {
            Length = length,
            IncludeLowerCaseLetters = true,
            IncludeUpperCaseLetters = true
        });

        // Assert
        Assert.Equal(length, password.Length);
        var chars = password.ToCharArray();
        Assert.DoesNotContain(chars, CharHelpers.IsEmpty);
        Assert.Contains(chars, char.IsUpper);
        Assert.Contains(chars, char.IsLower);
    }

    [Fact]
    public void Should_Return_Password_With_Upper_Lower_And_Digits()
    {
        // Arrange
        const int length = 10;
        _charDistributionMock
            .Setup(x => x.Distribute(It.IsAny<IEnumerable<string>>(), length))
            .Returns(new Dictionary<string, int>()
            {
                { "LowerCase", 4 },
                { "UpperCase", 3 },
                { "Digits", 3 }
            });

        // Act
        var password = _passwordGenerator.Generate(new PasswordOptions
        {
            Length = length,
            IncludeLowerCaseLetters = true,
            IncludeUpperCaseLetters = true,
            IncludeDigits = true
        });

        // Assert
        Assert.Equal(length, password.Length);
        var chars = password.ToCharArray();
        Assert.DoesNotContain(chars, CharHelpers.IsEmpty);
        Assert.Contains(chars, char.IsUpper);
        Assert.Contains(chars, char.IsLower);
        Assert.Contains(chars, char.IsDigit);
    }
}
