using FsCheck;
using FsCheck.Xunit;
using Ploeh.Samples.BookingApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyBasedTesting.Tests;

public class ARealLifeExample
{
    [Property]
    public void CanAcceptWhenCapacityIsSufficient_1(
    NonNegativeInt capacitySurplus,
    PositiveInt quantity,
    PositiveInt[] reservationQantities, DateTime reservationDate)
    {
        var date = reservationDate;
        var reservation = new Reservation
        {
            Date = date,
            Quantity = quantity.Item
        };
        var reservedSeats = reservationQantities.Sum(x => x.Item);
        var capacity = reservedSeats + quantity.Item + capacitySurplus.Item;
        var sut = new MaîtreD(capacity);

        var reservations =
            reservationQantities.Select(q => new Reservation { Quantity = q.Item, Date = date });
        var actual = sut.CanAccept(reservations, reservation);

        Assert.True(actual);
    }

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


    [Property]
    public Property CanAcceptWhenCapacityIsSufficient_2()
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

    public static Arbitrary<Reservation> Reservations() =>
           Arb.From(GenerateReservation);

    [Property(Arbitrary = [typeof(ARealLifeExample)])]
    public void CanAcceptWhenCapacityIsSufficient_3(IList<Reservation> reservations, Reservation reservation, NonNegativeInt capacitySurplus)
    {
        var reservedSeats = reservations.Sum(r => r.Quantity);
        var capacity =
            reservation.Quantity + reservedSeats + capacitySurplus.Item;
        var sut = new MaîtreD(capacity);

        var actual = sut.CanAccept(reservations, reservation);

        Assert.True(actual);
    }

    [Property]
    public void CanAcceptOnInsufficientCapacity_1(
    NonNegativeInt capacitySurplus,
    PositiveInt excessQuantity,
    PositiveInt[] reservationQantities, DateTime reservationDate)
    {
        var date = reservationDate;
        var reservation = new Reservation
        {
            Date = date,
            Quantity = capacitySurplus.Item + excessQuantity.Item
        };
        var reservedSeats = reservationQantities.Sum(x => x.Item);
        var capacity = reservedSeats + capacitySurplus.Item;

        var sut = new MaîtreD(capacity);

        var reservations =
           reservationQantities.Select(q => new Reservation { Quantity = q.Item, Date = date });

        var actual = sut.CanAccept(reservations, reservation);

        Assert.False(actual);
    }

    [Property]
    public Property CanAcceptOnInsufficientCapacity_2()
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
}
