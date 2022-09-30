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

        var systemSharedRandomNumberGenerator = new SystemSharedRandomNumberGenerator();
        _passwordGenerator = new PasswordGenerator(
            _charDistributionMock.Object,
            new Dictionary<PasswordCharType, IRandomCharacterProvider>
            {
                { PasswordCharType.LowerCase, new RandomLowerCaseLetterProvider(systemSharedRandomNumberGenerator) },
                { PasswordCharType.UpperCase, new RandomUpperCaseLetterProvider(systemSharedRandomNumberGenerator) },
                { PasswordCharType.Digit, new RandomDigitProvider(systemSharedRandomNumberGenerator) },
                { PasswordCharType.Symbol, new RandomSymbolProvider(systemSharedRandomNumberGenerator) }
            },
            systemSharedRandomNumberGenerator
        );
    }

    [Theory]
    [InlineData(5)]
    [InlineData(10)]
    [InlineData(20)]
    public void Should_Return_Password_Of_Requested_Length(int length)
    {
        // Arrange
        _charDistributionMock
            .Setup(x => x.Distribute(It.IsAny<IEnumerable<PasswordCharType>>(), length))
            .Returns(new Dictionary<PasswordCharType, int>()
            {
                { PasswordCharType.LowerCase, length }
            });

        // Act
        var password = _passwordGenerator.Generate(new PasswordOptions
        {
            Length = length,
            IncludeLowerCaseLetters = true
        });

        // Assert
        Assert.Equal(length, password.Length);
        AssertHelpers.Count(password, char.IsLower, length);
        AssertHelpers.Count(password, char.IsUpper, 0);
        AssertHelpers.Count(password, char.IsDigit, 0);
        AssertHelpers.Count(password, char.IsSymbol, 0);
    }

    [Fact]
    public void Should_Return_Password_Containing_Only_LowerLetters_If_No_Character_Type_Included()
    {
        // Arrange
        const int length = 10;

        _charDistributionMock
            .Setup(x => x.Distribute(It.IsAny<IEnumerable<PasswordCharType>>(), length))
            .Returns(new Dictionary<PasswordCharType, int>()
            {
                { PasswordCharType.LowerCase, length }
            });

        // Act
        var password = _passwordGenerator.Generate(new PasswordOptions
        {
            Length = length
        });

        // Assert
        Assert.Equal(length, password.Length);
        AssertHelpers.Count(password, char.IsLower, length);
        AssertHelpers.Count(password, char.IsUpper, 0);
        AssertHelpers.Count(password, char.IsDigit, 0);
        AssertHelpers.Count(password, char.IsSymbol, 0);
    }

    [Fact]
    public void Should_Return_Password_With_LowerLetters()
    {
        // Arrange
        const int length = 10;
        _charDistributionMock
            .Setup(x => x.Distribute(It.IsAny<IEnumerable<PasswordCharType>>(), length))
            .Returns(new Dictionary<PasswordCharType, int>()
            {
                { PasswordCharType.LowerCase, 10 }
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
        AssertHelpers.Count(password.ToCharArray(), char.IsLower, length);
    }

    [Fact]
    public void Should_Return_Password_With_UpperLetters()
    {
        // Arrange
        const int length = 10;
        _charDistributionMock
            .Setup(x => x.Distribute(It.IsAny<IEnumerable<PasswordCharType>>(), length))
            .Returns(new Dictionary<PasswordCharType, int>()
            {
                { PasswordCharType.UpperCase, 10 }
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
        AssertHelpers.Count(password.ToCharArray(), char.IsUpper, length);
    }

    [Fact]
    public void Should_Return_Password_With_MixedLetters()
    {
        // Arrange
        const int length = 10;
        _charDistributionMock
            .Setup(x => x.Distribute(It.IsAny<IEnumerable<PasswordCharType>>(), length))
            .Returns(new Dictionary<PasswordCharType, int>()
            {
                { PasswordCharType.LowerCase, 5 },
                { PasswordCharType.UpperCase, 5 }
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
        AssertHelpers.Count(chars, char.IsUpper, 5);
        AssertHelpers.Count(chars, char.IsLower, 5);
    }

    [Fact]
    public void Should_Return_Password_With_Upper_Lower_And_Digits()
    {
        // Arrange
        const int length = 10;
        _charDistributionMock
            .Setup(x => x.Distribute(It.IsAny<IEnumerable<PasswordCharType>>(), length))
            .Returns(new Dictionary<PasswordCharType, int>()
            {
                { PasswordCharType.LowerCase, 4 },
                { PasswordCharType.UpperCase, 3 },
                { PasswordCharType.Digit, 3 }
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
        AssertHelpers.Count(chars, char.IsUpper, 3);
        AssertHelpers.Count(chars, char.IsLower, 4);
        AssertHelpers.Count(chars, char.IsDigit, 3);
    }

    [Fact]
    public void Should_Return_Password_With_Upper_Lower_Digits_And_Symbols()
    {
        // Arrange
        const int length = 10;
        _charDistributionMock
            .Setup(x => x.Distribute(It.IsAny<IEnumerable<PasswordCharType>>(), length))
            .Returns(new Dictionary<PasswordCharType, int>()
            {
                { PasswordCharType.LowerCase, 3 },
                { PasswordCharType.UpperCase, 3 },
                { PasswordCharType.Digit, 2 },
                { PasswordCharType.Symbol, 2 },
            });

        // Act
        var password = _passwordGenerator.Generate(new PasswordOptions
        {
            Length = length,
            IncludeLowerCaseLetters = true,
            IncludeUpperCaseLetters = true,
            IncludeDigits = true,
            IncludeSymbols = true
        });

        // Assert
        Assert.Equal(length, password.Length);
        var chars = password.ToCharArray();
        Assert.DoesNotContain(chars, CharHelpers.IsEmpty);
        AssertHelpers.Count(chars, char.IsUpper, 3);
        AssertHelpers.Count(chars, char.IsLower, 3);
        AssertHelpers.Count(chars, char.IsDigit, 2);
        AssertHelpers.Count(chars, CharHelpers.IsPasswordSymbol, 2);
    }
}
