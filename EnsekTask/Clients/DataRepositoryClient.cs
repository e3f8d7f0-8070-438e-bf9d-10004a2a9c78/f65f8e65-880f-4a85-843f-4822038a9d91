using EnsekTask.Configuration;
using EnsekTask.Extensions;
using EnsekTask.Models.Entities;
using Microsoft.Data.Sqlite;

namespace EnsekTask.Clients
{
    public class DataRepositoryClient(ILogger<DataRepositoryClient> logger, IDataRepositoryConfiguration configuration) : IDataRepositoryClient
    {
        //  whilst we can have multiple concurrent readers with SQLite
        //  we can only have one writer. Protecting it with this.
        private readonly SemaphoreSlim _semaphoreWriter = new(1);

        public async Task<bool> AddMeterReadingAsync(MeterReading reading)
        {
            if (!await AccountExistsAsync(reading.AccountNumber))
            {
                logger.LogWarning("Account not found: {AccountNumber}",
                    reading.AccountNumber);
                return false;
            }

            if (await IsIdenticalMeterReadingAsync(reading))
            {
                logger.LogWarning("Identical reading found for account: {AccountNumber}",
                    reading.AccountNumber);
                return false;
            }

            var mostRecentMeterReadingDate = await GetMostRecentMeterReadingDateAsync(reading.AccountNumber);
            if (mostRecentMeterReadingDate != null && mostRecentMeterReadingDate > reading.Date)
            {
                logger.LogWarning("Reading found with later date for account: {AccountNumber}",
                    reading.AccountNumber);
                return false;
            }

            var result = await WriteMeterReadingAsync(reading);

            return result;
        }

        internal async Task<bool> AccountExistsAsync(int accountNumber)
        {
            var result = false;

            var connection = new SqliteConnection(configuration.ConnectionString);
            await connection.OpenAsync();

            await using var command = new SqliteCommand("SELECT COUNT(1) FROM ACCOUNTS WHERE ACCOUNT_NUMBER = @accountNumber", connection);
            command.Parameters.AddWithValue("@accountNumber", accountNumber);
            var reader = await command.ExecuteReaderAsync();
            while (reader.Read())
            {
                var count = reader.GetInt32(0);
                result = (count != 0);
            }

            await connection.CloseAsync();
            await connection.DisposeAsync();

            return result;
        }

        internal async Task<bool> IsIdenticalMeterReadingAsync(MeterReading reading)
        {
            var result = false;

            var connection = new SqliteConnection(configuration.ConnectionString);
            await connection.OpenAsync();

            await using var command = new SqliteCommand("SELECT COUNT(1) FROM METER_READINGS WHERE ACCOUNT_NUMBER = @accountNumber AND DATE = @date AND READING = @reading", connection);
            command.Parameters.AddWithValue("@accountNumber", reading.AccountNumber);
            command.Parameters.AddWithValue("@date", reading.Date.ToTimestamp());
            command.Parameters.AddWithValue("@reading", reading.Reading);
            var reader = await command.ExecuteReaderAsync();
            while (reader.Read())
            {
                var count = reader.GetInt32(0);
                result = (count != 0);
            }

            await connection.CloseAsync();
            await connection.DisposeAsync();

            return result;
        }

        internal async Task<DateTime?> GetMostRecentMeterReadingDateAsync(int accountNumber)
        {
            var result = (DateTime?)null;

            var connection = new SqliteConnection(configuration.ConnectionString);
            await connection.OpenAsync();

            await using var command = new SqliteCommand("SELECT DATE FROM METER_READINGS WHERE ACCOUNT_NUMBER = @accountNumber ORDER BY DATE DESC LIMIT 1", connection);
            command.Parameters.AddWithValue("@accountNumber", accountNumber);
            var reader = await command.ExecuteReaderAsync();
            while (reader.Read())
            {
                var date = reader.GetInt32(0);
                result = date.FromTimestamp();
            }

            await connection.CloseAsync();
            await connection.DisposeAsync();

            return result;
        }

        internal async Task<bool> WriteMeterReadingAsync(MeterReading reading)
        {
            var result = false;

            await _semaphoreWriter.WaitAsync();
            try
            {
                var connection = new SqliteConnection(configuration.ConnectionString);
                await connection.OpenAsync();

                await using var command = new SqliteCommand("INSERT INTO METER_READINGS (ACCOUNT_NUMBER, DATE, READING) VALUES (@accountNumber, @date, @reading)", connection);
                command.Parameters.AddWithValue("@accountNumber", reading.AccountNumber);
                command.Parameters.AddWithValue("@date", reading.Date.ToTimestamp());
                command.Parameters.AddWithValue("@reading", reading.Reading);
                command.ExecuteNonQuery();

                await connection.CloseAsync();
                await connection.DisposeAsync();

                result = true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Database update failed");
            }
            _semaphoreWriter.Release();

            return result;
        }
    }
}
