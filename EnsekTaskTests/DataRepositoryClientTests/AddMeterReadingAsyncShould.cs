using EnsekTask.Models.Entities;
using NUnit.Framework;

namespace EnsekTaskTests.DataRepositoryClientTests
{
    public class AddMeterReadingAsyncShould : DataRepositoryClientTestsBase
    {
        [Test]
        public async Task Reject_MeterReading_When_AccountNotExists()
        {
            //  Arrange
            //  the database has just been emptied so there are no accounts
            var reading = new MeterReading
            {
                AccountNumber = 1,
                Date = DateTime.Today,
                Reading = 0
            };

            //  Act
            var result = await DataRepositoryClient.AddMeterReadingAsync(reading);

            //  Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public async Task Accept_MeterReading_When_AccountExists()
        {
            //  Arrange
            var account = new Account
            {
                AccountNumber = 1,
                FirstName = "Test",
                LastName = "Test",
            };
            await SeedAccountsAsync([account]);
            var reading = new MeterReading
            {
                AccountNumber = 1,
                Date = DateTime.Today,
                Reading = 0
            };

            //  Act
            var result = await DataRepositoryClient.AddMeterReadingAsync(reading);

            //  Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public async Task Reject_MeterReading_When_AccountExists_And_ExistingReading_Identical()
        {
            //  Arrange
            var account = new Account
            {
                AccountNumber = 1,
                FirstName = "Test",
                LastName = "Test",
            };
            await SeedAccountsAsync([account]);
            var reading = new MeterReading
            {
                AccountNumber = 1,
                Date = DateTime.Today,
                Reading = 0
            };
            await SeedMeterReadingsAsync([reading]);

            //  Act
            var result = await DataRepositoryClient.AddMeterReadingAsync(reading);

            //  Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public async Task Reject_MeterReading_When_AccountExists_And_ExistingReading_Later()
        {
            //  Arrange
            var account = new Account
            {
                AccountNumber = 1,
                FirstName = "Test",
                LastName = "Test",
            };
            await SeedAccountsAsync([account]);
            var readingFirst = new MeterReading
            {
                AccountNumber = 1,
                Date = DateTime.Today,
                Reading = 0
            };
            await SeedMeterReadingsAsync([readingFirst]);
            var readingSecond = new MeterReading
            {
                AccountNumber = 1,
                Date = DateTime.Today.AddDays(-1),
                Reading = 0
            };

            //  Act
            var result = await DataRepositoryClient.AddMeterReadingAsync(readingSecond);

            //  Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public async Task Accept_MeterReading_When_AccountExists_And_ExistingReading_Earlier()
        {
            //  Arrange
            var account = new Account
            {
                AccountNumber = 1,
                FirstName = "Test",
                LastName = "Test",
            };
            await SeedAccountsAsync([account]);
            var readingFirst = new MeterReading
            {
                AccountNumber = 1,
                Date = DateTime.Today.AddDays(-1),
                Reading = 0
            };
            await SeedMeterReadingsAsync([readingFirst]);
            var readingSecond = new MeterReading
            {
                AccountNumber = 1,
                Date = DateTime.Today,
                Reading = 0
            };

            //  Act
            var result = await DataRepositoryClient.AddMeterReadingAsync(readingSecond);

            //  Assert
            Assert.That(result, Is.True);
        }
    }
}
