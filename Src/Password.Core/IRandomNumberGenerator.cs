namespace Password.Core;

public interface IRandomNumberGenerator
{
    int Next(int minValue, int maxValue);
}

public class SystemSharedRandomNumberGenerator : IRandomNumberGenerator
{
    private readonly Random _random;

    public SystemSharedRandomNumberGenerator()
    {
        _random = new Random();
    }

    public int Next(int minValue, int maxValue) => _random.Next(minValue, maxValue);
}
