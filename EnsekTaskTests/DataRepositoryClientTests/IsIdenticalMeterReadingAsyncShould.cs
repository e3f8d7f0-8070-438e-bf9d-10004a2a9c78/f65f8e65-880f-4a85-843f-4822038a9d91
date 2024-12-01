using EnsekTask.Models.Entities;
using NUnit.Framework;

namespace EnsekTaskTests.DataRepositoryClientTests
{
    internal class IsIdenticalMeterReadingAsyncShould : DataRepositoryClientTestsBase
    {
        [Test]
        public async Task ReturnFalse_When_NoMatchExists()
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
                Date = DateTime.Today,
                Reading = 0
            };

            //  Act
            var result = await DataRepositoryClient.IsIdenticalMeterReadingAsync(reading);

            //  Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public async Task ReturnTrue_When_MatchExists()
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
                Date = DateTime.Today,
                Reading = 0
            };
            await SeedMeterReadingsAsync([reading]);

            //  Act
            var result = await DataRepositoryClient.IsIdenticalMeterReadingAsync(reading);

            //  Assert
            Assert.That(result, Is.True);
        }
    }
}
