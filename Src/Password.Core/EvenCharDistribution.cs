namespace Password.Core;

internal class EvenCharDistribution : ICharDistribution
{
    public Dictionary<PasswordCharType, int> Distribute(IEnumerable<PasswordCharType> charTypes, int totalPlaces)
    {
        if (charTypes is null)
        {
            throw new ArgumentNullException(nameof(charTypes));
        }

        var charTypeList = charTypes.ToList();
        var charTypeCount = charTypeList.Count;

        var minimumPlaces = totalPlaces / charTypeCount;
        var maxPlaces = minimumPlaces + 1;
        var remainingPlaces = totalPlaces % charTypeCount;

        var result = new Dictionary<PasswordCharType, int>(charTypeCount);
        foreach (var (index, type) in charTypeList.Select((item, index) => (index, item)))
        {
            result[type] = ShouldGetMaxPlaces(index) ? maxPlaces : minimumPlaces;
        }

        return result;

        bool ShouldGetMaxPlaces(int typeIndex) => typeIndex < remainingPlaces;
    }
}
