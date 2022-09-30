namespace Password.Core;

public interface ICharDistribution
{
    Dictionary<PasswordCharType, int> Distribute(IEnumerable<PasswordCharType> charTypes, int totalPlaces);
}
