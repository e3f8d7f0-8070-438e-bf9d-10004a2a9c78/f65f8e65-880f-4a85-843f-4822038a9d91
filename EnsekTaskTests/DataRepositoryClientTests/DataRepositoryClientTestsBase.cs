using EnsekTask.Clients;
using EnsekTask.Configuration;
using EnsekTask.Extensions;
using EnsekTask.Models.Entities;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace EnsekTaskTests.DataRepositoryClientTests
{
    public class DataRepositoryClientTestsBase
    {
        protected Mock<ILogger<DataRepositoryClient>> LoggerDataRepositoryClientMock = null!;
        protected DataRepositoryConfiguration DataRepositoryConfiguration = null!;
        protected DataRepositoryClient DataRepositoryClient = null!;
        protected MeterReading MeterReadingDayOne = null!;
        protected MeterReading MeterReadingDayTwo = null!;
        private const string _databaseFilename = "Data/meterReadings.sqlite";

        [SetUp]
        public async Task SetUpAsync()
        {
            LoggerDataRepositoryClientMock = new Mock<ILogger<DataRepositoryClient>>();
            DataRepositoryConfiguration = new DataRepositoryConfiguration
            {
                ConnectionString = $"Data Source={_databaseFilename}"
            };
            await WipeDatabaseAsync();

            DataRepositoryClient = new DataRepositoryClient(LoggerDataRepositoryClientMock.Object, DataRepositoryConfiguration);

            MeterReadingDayOne = new MeterReading
            {
                AccountNumber = 1,
                Date = new DateTime(2000, 1, 1),
                Reading = 12345
            };
            MeterReadingDayTwo = new MeterReading
            {
                AccountNumber = 1,
                Date = new DateTime(2000, 1, 2),
                Reading = 12345
            };
        }

        [TearDown]
        public async Task TearDownAsync()
        {
            await WipeDatabaseAsync();
        }

        private async Task WipeDatabaseAsync()
        {
            var connection = new SqliteConnection(DataRepositoryConfiguration.ConnectionString);
            await connection.OpenAsync();

            //  SQLite doesn't have a TRUNCATE TABLE command
            //  but DELETE with no WHERE clause invokes an optimisation that does the same thing
            //  https://www.techonthenet.com/sqlite/truncate.php
            await using var commandReadings = new SqliteCommand("DELETE FROM METER_READINGS", connection);
            commandReadings.ExecuteNonQuery();
            await using var commandAccounts = new SqliteCommand("DELETE FROM ACCOUNTS", connection);
            commandAccounts.ExecuteNonQuery();

            await connection.CloseAsync();
            await connection.DisposeAsync();
        }

        protected async Task SeedAccountsAsync(IEnumerable<Account> accounts)
        {
            var connection = new SqliteConnection(DataRepositoryConfiguration.ConnectionString);
            await connection.OpenAsync();
            foreach (var account in accounts)
            {
                await using var command = new SqliteCommand("INSERT INTO ACCOUNTS (ACCOUNT_NUMBER, FIRST_NAME, LAST_NAME) VALUES (@accountNumber, @firstName, @lastName)", connection);
                command.Parameters.AddWithValue("@accountNumber", account.AccountNumber);
                command.Parameters.AddWithValue("@firstName", account.FirstName);
                command.Parameters.AddWithValue("@lastName", account.LastName);
                command.ExecuteNonQuery();
            }
            await connection.CloseAsync();
            await connection.DisposeAsync();
        }

        protected async Task SeedMeterReadingsAsync(IEnumerable<MeterReading> readings)
        {
            var connection = new SqliteConnection(DataRepositoryConfiguration.ConnectionString);
            await connection.OpenAsync();
            foreach (var reading in readings)
            {
                await using var command = new SqliteCommand("INSERT INTO METER_READINGS (ACCOUNT_NUMBER, DATE, READING) VALUES (@accountNumber, @date, @reading)", connection);
                command.Parameters.AddWithValue("@accountNumber", reading.AccountNumber);
                command.Parameters.AddWithValue("@date", reading.Date.ToTimestamp());
                command.Parameters.AddWithValue("@reading", reading.Reading);
                command.ExecuteNonQuery();
            }
            await connection.CloseAsync();
            await connection.DisposeAsync();
        }
    }
}
