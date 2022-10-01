using Password.Core;
using Xunit;

namespace Password.UnitTests;

public class PasswordOptionsMergerTests
{
    [Fact]
    public void Should_Return_Default_Options_If_No_Override_Configuration_Given()
    {
        // Arrange
        var options = new PasswordOptions();

        // Act
        var mergedOptions = PasswordOptionsMerger.Merge(options);

        // Assert
        Assert.Equal(15, mergedOptions.Length);
        Assert.True(mergedOptions.IncludeLowerCaseLetters);
        Assert.True(mergedOptions.IncludeUpperCaseLetters);
        Assert.True(mergedOptions.IncludeDigits);
        Assert.True(mergedOptions.IncludeSymbols);
    }

    [Fact]
    public void Should_Return_Options_With_Overridden_Length_And_Rest_Default_Setting_If_Only_Length_Is_Given()
    {
        // Arrange
        var options = new PasswordOptions()
        {
            Length = 10
        };

        // Act
        var mergedOptions = PasswordOptionsMerger.Merge(options);

        // Assert
        Assert.Equal(10, mergedOptions.Length);
        Assert.True(mergedOptions.IncludeLowerCaseLetters);
        Assert.True(mergedOptions.IncludeUpperCaseLetters);
        Assert.True(mergedOptions.IncludeDigits);
        Assert.True(mergedOptions.IncludeSymbols);
    }

    [Theory]
    [InlineData(false, true, false, false)]
    [InlineData(false, false, true, false)]
    [InlineData(false, false, false, true)]
    [InlineData(false, false, true, true)]
    [InlineData(false, true, false, true)]
    [InlineData(false, true, true, true)]
    public void Should_Only_Allow_To_Set_IncludeLowerCase_False_If_At_Least_On_Of_Other_Char_Types_Are_True(
        bool includeLowerCase,
        bool includeUpperCase,
        bool includeDigit,
        bool includeSymbol)
    {
        // Arrange
        var options = new PasswordOptions()
        {
            Length = 10,
            IncludeLowerCaseLetters = includeLowerCase,
            IncludeUpperCaseLetters = includeUpperCase,
            IncludeDigits = includeDigit,
            IncludeSymbols = includeSymbol
        };

        // Act
        var mergedOptions = PasswordOptionsMerger.Merge(options);

        // Assert
        Assert.Equal(10, mergedOptions.Length);
        Assert.False(mergedOptions.IncludeLowerCaseLetters);
        Assert.Equal(includeUpperCase, mergedOptions.IncludeUpperCaseLetters);
        Assert.Equal(includeDigit, mergedOptions.IncludeDigits);
        Assert.Equal(includeSymbol, mergedOptions.IncludeSymbols);
    }
}
