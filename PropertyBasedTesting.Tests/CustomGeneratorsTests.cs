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

namespace PropertyBasedTesting.Tests
{
    public class CustomGeneratorsTests
    {
        [Property]
        public Property CanAcceptReturnsReservationInHappyPathScenario()
        {
            return Prop.ForAll((
                from rs in GenerateReservation.ListOf()
                from r in GenerateReservation
                from cs in Arb.Default.NonNegativeInt().Generator
                select (rs, r, cs.Item)).ToArbitrary(),
                x =>
                {
                    var (reservations, reservation, capacitySurplus) = x;
                    var reservedSeats = reservations.Sum(r => r.Quantity);
                    var capacity =
                        reservation.Quantity + reservedSeats + capacitySurplus;
                    var sut = new MaîtreD(capacity);

                    var actual = sut.CanAccept(reservations, reservation);

                    Assert.True(actual);
                });
        }

        [Property]
        public Property CanAcceptOnInsufficientCapacity()
        {
            return Prop.ForAll((
                from r in GenerateReservation
                from eq in Arb.Default.PositiveInt().Generator
                from cs in Arb.Default.NonNegativeInt().Generator
                from rs in GenerateReservation.ListOf()
                select (r, eq.Item, cs.Item, rs)).ToArbitrary(),
                x =>
                {
                    var (reservation, excessQuantity, capacitySurplus, reservations) = x;
                    var reservedSeats = reservations.Sum(r => r.Quantity);
                    var quantity = capacitySurplus + excessQuantity;
                    var capacity = capacitySurplus + reservedSeats;
                    reservation.Quantity = quantity;
                    var sut = new MaîtreD(capacity);

                    var actual = sut.CanAccept(reservations, reservation);

                    Assert.False(actual);
                });
        }

        //Will register all static methods that return an Arbitrary
        [Property(Arbitrary = new[] { typeof(CustomGeneratorsTests) }, Verbose = true)]
        public Property BirthdayTester(MonthOfYear monthOfYear)
        {
            return (monthOfYear.Month != 2).ToProperty();
        }

        //TODO: Datum creator maken
        //Will register all static methods that return an Arbitrary
        [Property(Arbitrary = new[] { typeof(CustomGeneratorsTests) }, Verbose = true)]
        public Property InvalidDateTester(CustomDateTime dateTime)
        {
            return DateTime.TryParse($"{dateTime.Year}-{dateTime.Month}-{dateTime.Day}", out var datetime).ToProperty();
        }


        //TODO: Datum creator maken
        //Will register all static methods that return an Arbitrary
        [Property(Arbitrary = new[] { typeof(CustomGeneratorsTests) }, Verbose = true)]
        public Property IntTester(int x)
        {
            return (x<81).ToProperty();
        }

        private static Gen<Int32> GenerateIntegers =>
             from value in Gen.Choose(-100, +100)
             select value;



        private static Gen<Reservation> GenerateReservation =>
            from d in Arb.Default.DateTime().Generator
            from e in Arb.Default.NonWhiteSpaceString().Generator
            from n in Arb.Default.NonWhiteSpaceString().Generator
            from q in Arb.Default.PositiveInt().Generator
            select new Reservation
            {
                Date = d,
                Email = e.Item,
                Name = n.Item,
                Quantity = q.Item
            };

        private static Gen<MonthOfYear> GenerateMonthOfYear =>
            from month in Gen.Choose(1, 12)
            from year in Gen.Choose(1982, 2023)
            select new MonthOfYear(month, year);

        private static Gen<CustomDateTime> GenerateDateTime =>
           from day in Gen.Choose(1, 31)
           from month in Gen.Choose(1, 12)
           from year in Gen.Choose(1982, 2023)
           select new CustomDateTime(day, month, year);


        public static Arbitrary<MonthOfYear> MonthOfYear() =>
           Arb.From(GenerateMonthOfYear, MonthOfYearShrinker);

        public static Arbitrary<CustomDateTime> CustomDateTime() =>
           Arb.From(GenerateDateTime,CustomDateTimeShrinker);

        public static Arbitrary<Int32> CustomInt() =>
           Arb.From(GenerateIntegers, CustomIntegerShrinker2);

        public static IEnumerable<Int32> CustomIntegerShrinker(int current)
        {
            for (int i = 0; i < current; i++)
            {
                yield return i;
            }
        }

        public static IEnumerable<Int32> CustomIntegerShrinker2(int current)
        {
            for (int i = current; i > -100; --i)
            {
                yield return --i;
            }
        }

        public static IEnumerable<MonthOfYear> MonthOfYearShrinker(MonthOfYear monthOfYear)
        {
            yield return new MonthOfYear(monthOfYear.Month - 1, monthOfYear.Year);
            yield return new MonthOfYear(monthOfYear.Month + 1, monthOfYear.Year);
            yield return new MonthOfYear(monthOfYear.Month, monthOfYear.Year + 1);
            yield return new MonthOfYear(monthOfYear.Month, monthOfYear.Year - 1);
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
                yield return new CustomDateTime(newDt.Day, newDt.Month, newDt.Year+1); ;
            }
        }

    }

    public record MonthOfYear(int Month, int Year);

    public record CustomDateTime(int Day, int Month, int Year);
}
