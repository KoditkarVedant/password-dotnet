namespace Password.Core;

public interface ICharDistribution
{
    Dictionary<string, int> Distribute(IEnumerable<string> charTypes, int totalPlaces);
}
