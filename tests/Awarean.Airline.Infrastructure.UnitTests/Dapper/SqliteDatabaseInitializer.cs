using System.Data;
using Awarean.Airline.Infrastructure.Dapper.Context;
using Awarean.Airline.Domain.Entities;
using System.Data.SQLite;

namespace Awarean.Airline.Infrastructure.UnitTests;

public class SqliteDatabaseInitializer : DatabaseInitializer
{
    public override string ConnectionStringName => "Data Source=..//..//..//Database.db";

    public override IEnumerable<string> CreationScripts => new List<string>()
    {
       // "CREATE DATABASE TestAirlineDatabase;",

        @$"CREATE TABLE IF NOT EXISTS {nameof(Aircraft)}s (
            {nameof(Aircraft.Id)} INTEGER PRIMARY KEY AUTOINCREMENT,
            {nameof(Aircraft.ActualParkingLocation)} TEXT ,
            {nameof(Aircraft.AircraftType)} TEXT,
            {nameof(Aircraft.CreatedAt)} TEXT,
            {nameof(Aircraft.UpdatedAt)} TEXT ,
            {nameof(Aircraft.Model)} TEXT
        );",

        @$"CREATE TABLE IF NOT EXISTS {nameof(Flight)}s (
            {nameof(Flight.Id)} INTEGER PRIMARY KEY AUTOINCREMENT,
            {nameof(Flight.Departure)} TEXT ,
            {nameof(Flight.Arrival)} TEXT,
            {nameof(Flight.DepartureAirport)} TEXT ,
            {nameof(Flight.ArrivalAirport)} TEXT,
            {nameof(Flight.CreatedAt)} TEXT,
            {nameof(Flight.UpdatedAt)} TEXT ,
            {nameof(Flight.AircraftId)} INTEGER NULL,
            FOREIGN KEY({nameof(Flight.AircraftId)}) REFERENCES {nameof(Aircraft)}s({nameof(Aircraft.Id)})
        );",

        @$"CREATE TABLE IF NOT EXISTS {nameof(Aircraft)}_{nameof(Flight)}s (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            FlightId INTEGER REFERENCES {nameof(Flight)}s({nameof(Flight.Id)}),
            AircraftId INTEGER REFERENCES {nameof(Aircraft)}s({nameof(Aircraft.Id)}) 
        );",
    };

    public override IDbConnection GetConnection() => new SQLiteConnection(ConnectionStringName);
}