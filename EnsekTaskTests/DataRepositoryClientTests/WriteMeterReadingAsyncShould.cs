using EnsekTask.Models.Entities;
using Microsoft.Data.Sqlite;
using NUnit.Framework;

namespace EnsekTaskTests.DataRepositoryClientTests
{
    internal class WriteMeterReadingAsyncShould : DataRepositoryClientTestsBase
    {
        //  there are several tests that could be added for non-happy paths
        //  but that would probably be overkill because everything is defensive
        //  before it reaches the code being tested here

        [Test]
        public async Task AddMeterReadingToDatabase_When_AllCriteriaMet()
        {
            //  Arrange
            var account = new Account
            {
                AccountNumber = 1,
                FirstName = "Test",
                LastName = "Test",
            };
            await SeedAccountsAsync([account]);
            var reading = new MeterReading()
            {
                AccountNumber = 1,
                Date = DateTime.Today.AddDays(-1),
                Reading = 0
            };

            //  Act
            var result = await DataRepositoryClient.WriteMeterReadingAsync(reading);

            //  Assert
            Assert.That(result, Is.True);

            //  check it was actually written to the database
            var connection = new SqliteConnection(DataRepositoryConfiguration.ConnectionString);
            var dataWritten = false;
            try
            {
                await connection.OpenAsync();

                await using var command =
                    new SqliteCommand("SELECT COUNT(1) FROM METER_READINGS WHERE ACCOUNT_NUMBER = @accountNumber",
                        connection);
                command.Parameters.AddWithValue("@accountNumber", account.AccountNumber);
                var reader = await command.ExecuteReaderAsync();
                while (reader.Read())
                {
                    var count = reader.GetInt32(0);
                    dataWritten = (count == 1);
                }
            }
            finally
            {
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }

            Assert.That(dataWritten, Is.True);
        }
    }
}
