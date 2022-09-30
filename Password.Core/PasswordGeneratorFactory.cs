namespace Password.Core;

public static class PasswordGeneratorFactory
{
    public static IPasswordGenerator GetGenerator()
    {
        var systemSharedRandomNumberGenerator = new SystemSharedRandomNumberGenerator();
        return new PasswordGenerator(
            new EvenCharDistribution(),
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
}
