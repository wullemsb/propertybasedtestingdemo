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
using FsCheck.Fluent;

namespace PropertyBasedTesting.Tests
{
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
           Arb.From(GenerateIntegers, CustomIntegerShrinker2);


        public static IEnumerable<Int32> IntegerShrinker(int current)
        {
            int i = current;

            while (i > 0)
            {
                --i;
                yield return i;
            }
        }

        public static IEnumerable<Int32> CustomIntegerShrinker(int current)
        {
            for (int i = 0; i < current; i++)
            {
                yield return i;
            }
        }

        public static IEnumerable<Int32> CustomIntegerShrinker2(int current)
        {
            var random=new System.Random();
            var increment = random.Next(5);
            var i = 0;

            while (i < current)
            {
                yield return i;
                i += increment;
            }
        }


        public static IEnumerable<CustomDateTime> CustomDateTimeShrinker(CustomDateTime dt)
        {
            for (; ; )
            {
                var newDt = dt;
                if (newDt.Day > 31 || newDt.Month > 12 || newDt.Year > 2023)
                    yield break;

                yield return new CustomDateTime(newDt.Day + 1, newDt.Month, newDt.Year); ;
                yield return new CustomDateTime(newDt.Day, newDt.Month + 1, newDt.Year); ;
                yield return new CustomDateTime(newDt.Day, newDt.Month, newDt.Year + 1); ;
            }
        }

    }

    public record CustomDateTime(int Day, int Month, int Year);
}
