﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ploeh.Samples.BookingApi;

public class MaîtreD
{
    public MaîtreD(int capacity)
    {
        Capacity = capacity;
    }

    public int Capacity { get; }

    public bool CanAccept(
        IEnumerable<Reservation> reservations,
        Reservation reservation)
    {
        var reservedSeats = reservations.Sum(r => r.Quantity);

        if (Capacity < reservedSeats + reservation.Quantity)
            return false;

        return true;
    }
}
