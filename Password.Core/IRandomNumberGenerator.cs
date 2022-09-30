namespace Password.Core;

public interface IRandomNumberGenerator
{
    int Next(int minValue, int maxValue);
}

public class SystemSharedRandomNumberGenerator : IRandomNumberGenerator
{
    public int Next(int minValue, int maxValue) => Random.Shared.Next(minValue, maxValue);
}
