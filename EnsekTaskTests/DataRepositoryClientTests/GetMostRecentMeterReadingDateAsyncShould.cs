using EnsekTask.Models.Entities;
using NUnit.Framework;

namespace EnsekTaskTests.DataRepositoryClientTests
{
    internal class GetMostRecentMeterReadingDateAsyncShould : DataRepositoryClientTestsBase
    {
        [Test]
        public async Task ReturnNull_When_NoReadingExists()
        {
            //  Arrange
            var account = new Account
            {
                AccountNumber = 1,
                FirstName = "Test",
                LastName = "Test",
            };
            await SeedAccountsAsync([account]);

            //  Act
            var result = await DataRepositoryClient.GetMostRecentMeterReadingDateAsync(account.AccountNumber);

            //  Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task ReturnCorrectDate_When_ReadingsExists()
        {
            //  Arrange
            var today = DateTime.Today;
            var account = new Account
            {
                AccountNumber = 1,
                FirstName = "Test",
                LastName = "Test",
            };
            await SeedAccountsAsync([account]);
            var reading1 = new MeterReading()
            {
                AccountNumber = 1,
                Date = DateTime.Today.AddDays(-1),
                Reading = 0
            };
            var reading2 = new MeterReading()
            {
                AccountNumber = 1,
                Date = today,
                Reading = 0
            };
            await SeedMeterReadingsAsync([reading1, reading2]);

            //  Act
            var result = await DataRepositoryClient.GetMostRecentMeterReadingDateAsync(account.AccountNumber);

            //  Assert
            Assert.That(result, Is.EqualTo(today));
        }
    }
}
