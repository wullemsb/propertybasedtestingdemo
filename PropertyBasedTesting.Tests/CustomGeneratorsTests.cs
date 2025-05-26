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





        private static Gen<Reservation> GenerateReservation =>
          from d in ArbMap.Default.GeneratorFor<DateTime>()
          from e in ArbMap.Default.GeneratorFor<NonWhiteSpaceString>()
          from n in ArbMap.Default.GeneratorFor<NonWhiteSpaceString>()
          from q in ArbMap.Default.GeneratorFor<PositiveInt>()
          select new Reservation
          {
              Date = d,
              Email = e.Item,
              Name = n.Item,
              Quantity = q.Item
          };


        [Property]
        public Property CanAcceptReturnsReservationInHappyPathScenario()
        {
            return Prop.ForAll((
                from rs in GenerateReservation.ListOf()
                from r in GenerateReservation
                from cs in ArbMap.Default.GeneratorFor<NonNegativeInt>()
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
                from eq in ArbMap.Default.GeneratorFor<PositiveInt>()
                from cs in ArbMap.Default.GeneratorFor<NonNegativeInt>()
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

      


    }
}