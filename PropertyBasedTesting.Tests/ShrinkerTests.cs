using FsCheck.Xunit;
using FsCheck;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PropertyBasedTesting.Calculator;
using Ploeh.Samples.BookingApi;
using System.Reflection.Emit;

namespace PropertyBasedTesting.Tests;

public class ShrinkerTests
{
    //Will register all static methods that return an Arbitrary
    [Property(Arbitrary = new[] { typeof(ShrinkerTests) }, Verbose = true)]
    public Property InvalidDateTester(CustomDateTime dateTime)
    {
        return DateTime.TryParse($"{dateTime.Year}-{dateTime.Month}-{dateTime.Day}", out var datetime).ToProperty();

    }

    //Will register all static methods that return an Arbitrary
    [Property(Arbitrary = new[] { typeof(ShrinkerTests) }, Verbose = true)]
    public Property IntTester(int x) => (x < 81).ToProperty();


    private static Gen<Int32> GenerateIntegers =>
         from value in Gen.Choose(-100, +100)
         select value;


    private static Gen<CustomDateTime> GenerateDateTime =>
       from day in Gen.Choose(1, 31)
       from month in Gen.Choose(1, 12)
       from year in Gen.Choose(1982, 2023)
       select new CustomDateTime(day, month, year);


    public static Arbitrary<CustomDateTime> CustomDateTime() =>
       Arb.From(GenerateDateTime, CustomDateTimeShrinker);

    public static Arbitrary<Int32> Int() =>
       Arb.From(GenerateIntegers, IntegerShrinker);


    public static IEnumerable<Int32> IntegerShrinker(int value)
    {
        if (value == 0)
            yield break;

        // Try zero first
        yield return 0;

        // Then try halfway to zero
        int mid = value / 2;
        if (mid != 0)
            yield return mid;

        // Then decrement/increment toward zero
        int step = value > 0 ? -1 : 1;
        for (int i = value + step; i != 0; i += step)
            yield return i;
    }

    public static IEnumerable<CustomDateTime> CustomDateTimeShrinker(CustomDateTime dt)
    {
        // Shrink day toward 1
        for (int d = dt.Day - 1; d >= 1; d--)
            yield return dt with { Day = d };

        // Shrink month toward 1
        for (int m = dt.Month - 1; m >= 1; m--)
            yield return dt with { Month = m };

        // Shrink year toward 1982
        for (int y = dt.Year - 1; y >= 1982; y--)
            yield return dt with { Year = y };
    }

}

public record CustomDateTime(int Day, int Month, int Year);
