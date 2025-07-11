﻿using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ploeh.Samples.BookingApi;

public class SqlReservationsRepository : IReservationsRepository
{
    public SqlReservationsRepository(string connectionString)
    {
        ConnectionString = connectionString;
    }

    public string ConnectionString { get; }

    public IEnumerable<Reservation> ReadReservations(DateTime date)
    {
        return ReadReservations(
            date.Date,
            date.Date.AddDays(1).AddTicks(-1));
    }

    private Reservation[] ReadReservations(DateTime min, DateTime max)
    {
        var result = new List<Reservation>();

        using (var conn = new SqlConnection(ConnectionString))
        using (var cmd = new SqlCommand(readByRangeSql, conn))
        {
            cmd.Parameters.Add(new SqlParameter("@MinDate", min));
            cmd.Parameters.Add(new SqlParameter("@MaxDate", max));

            conn.Open();
            using (var rdr = cmd.ExecuteReader())
            {
                while (rdr.Read())
                    result.Add(
                        new Reservation
                        {
                            Date = (DateTime) rdr["Date"],
                            Name = (string) rdr["Name"],
                            Email = (string) rdr["Email"],
                            Quantity = (int) rdr["Quantity"]
                        });
            }
        }

        return result.ToArray();
    }

    private const string readByRangeSql = @"
            SELECT [Date], [Name], [Email], [Quantity]
            FROM [dbo].[Reservations]
            WHERE YEAR(@MinDate) <= YEAR([Date])
            AND MONTH(@MinDate) <= MONTH([Date])
            AND DAY(@MinDate) <= DAY([Date])
            AND YEAR([Date]) <= YEAR(@MaxDate)
            AND MONTH([Date]) <= MONTH(@MaxDate)
            AND DAY([Date]) <= DAY(@MaxDate)";

    public int Create(Reservation reservation)
    {
        using (var conn = new SqlConnection(ConnectionString))
        using (var cmd = new SqlCommand(createReservationSql, conn))
        {
            cmd.Parameters.Add(
                new SqlParameter("@Date", reservation.Date));
            cmd.Parameters.Add(
                new SqlParameter("@Name", reservation.Name));
            cmd.Parameters.Add(
                new SqlParameter("@Email", reservation.Email));
            cmd.Parameters.Add(
                new SqlParameter("@Quantity", reservation.Quantity));

            conn.Open();
            return (int)cmd.ExecuteScalar();
        }
    }

    private const string createReservationSql = @"
            INSERT INTO [dbo].[Reservations] ([Date], [Name], [Email], [Quantity])
            OUTPUT INSERTED.Id
            VALUES (@Date, @Name, @Email, @Quantity)";
}
