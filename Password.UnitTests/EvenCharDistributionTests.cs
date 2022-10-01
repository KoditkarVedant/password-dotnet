using System.Linq;
using Password.Core;
using Xunit;

namespace Password.UnitTests;

public class EvenCharDistributionTests
{
    private readonly ICharDistribution _evenCharDistribution;

    public EvenCharDistributionTests()
    {
        _evenCharDistribution = new EvenCharDistribution();
    }

    [Theory]
    [MemberData(nameof(TestData_Should_Distribute_Evenly_To_All_If_There_Are_No_Odd_Places))]
    public void Should_Distribute_Evenly_To_All_If_There_Are_No_Odd_Places(
        int totalPlaces,
        PasswordCharType[] buckets,
        int placesPerCharType)
    {
        // Act
        var distribution = _evenCharDistribution.Distribute(buckets, totalPlaces);

        // Assert
        foreach (var bucket in buckets)
        {
            Assert.Equal(placesPerCharType, distribution[bucket]);
        }
    }

    private static object[] TestData_Should_Distribute_Evenly_To_All_If_There_Are_No_Odd_Places()
    {
        return new object[]
        {
            new object[]
            {
                8,
                new[]
                {
                    PasswordCharType.LowerCase,
                    PasswordCharType.UpperCase,
                    PasswordCharType.Symbol,
                    PasswordCharType.Digit
                },
                2
            },
            new object[]
            {
                9,
                new[]
                {
                    PasswordCharType.LowerCase, PasswordCharType.UpperCase, PasswordCharType.Symbol
                },
                3
            }
        };
    }

    [Theory]
    [MemberData(nameof(TestData_Should_Distribute_Odd_Placed_In_Round_Robin_Fashion))]
    public void Should_Distribute_Odd_Placed_In_Round_Robin_Fashion(
        int totalPlaces,
        PasswordCharType[] buckets,
        int[] placesPerBucket)
    {
        // Act
        var distribution = _evenCharDistribution.Distribute(buckets, totalPlaces);

        // Assert
        foreach (var (index, bucket) in buckets.Select((item, index) => (index, item)))
        {
            Assert.Equal(placesPerBucket[index], distribution[bucket]);
        }
    }

    private static object[] TestData_Should_Distribute_Odd_Placed_In_Round_Robin_Fashion()
    {
        return new object[]
        {
            new object[]
            {
                10,
                new PasswordCharType[]
                {
                    PasswordCharType.LowerCase,
                    PasswordCharType.UpperCase,
                    PasswordCharType.Digit,
                    PasswordCharType.Symbol,
                },
                new int[]
                {
                    3,
                    3,
                    2,
                    2,
                },
            },
            new object[]
            {
                15,
                new PasswordCharType[]
                {
                    PasswordCharType.LowerCase,
                    PasswordCharType.UpperCase,
                    PasswordCharType.Digit,
                    PasswordCharType.Symbol,
                },
                new int[]
                {
                    4,
                    4,
                    4,
                    3,
                },
            }
        };
    }
}
