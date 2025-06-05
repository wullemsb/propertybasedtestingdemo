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

public class CustomGeneratorsTests
{
    public record CustomDateTime(int Day, int Month, int Year);
   
    private static Gen<CustomDateTime> GenerateDateTime =>
            from day in Gen.Choose(1, 31)
            from month in Gen.Choose(1, 12)
            from year in Gen.Choose(1982, 2023)
            select new CustomDateTime(day, month, year);

    public static Arbitrary<CustomDateTime> CustomDateTimes() =>
       Arb.From(GenerateDateTime);


    //Will register all static methods that return an Arbitrary
    [Property(Arbitrary = new[] { typeof(CustomGeneratorsTests) }, Verbose = true)]
    public Property InvalidDateTester(CustomDateTime dateTime)
    {
        return DateTime.TryParse($"{dateTime.Year}-{dateTime.Month}-{dateTime.Day}", out var datetime).ToProperty();
    }







    private static Gen<Int32> GeneratePositiveIntegers =>
         from value in Gen.Choose(-100, +100)
         where value > 0
         select value;

    public static Arbitrary<Int32> CustomInt() =>
            Arb.From(GeneratePositiveIntegers);


    //Will register all static methods that return an Arbitrary
    [Property(Arbitrary = new[] { typeof(CustomGeneratorsTests) }, Verbose = true)]
    public Property IntTester(int x) => (x < 81).ToProperty();

}