using System;
using System.Collections.Generic;
using System.Linq;
using Xunit.Sdk;

namespace Password.UnitTests;

public static class AssertHelpers
{
    public static void Count<T>(IEnumerable<T> collection, Predicate<T> check, int times)
    {
        ArgumentNullException.ThrowIfNull(collection, nameof(collection));
        ArgumentNullException.ThrowIfNull(check, nameof(check));

        var timesSucceeded = collection.Count(item => check(item));

        if (timesSucceeded != times)
        {
            throw new AssertCollectionCountException(times, timesSucceeded);
        }
    }
}
