using EnsekTask.Models.Entities;
using NUnit.Framework;

namespace EnsekTaskTests.DataRepositoryClientTests
{
    internal class AccountExistsAsyncShould : DataRepositoryClientTestsBase
    {
        [Test]
        public async Task ReturnFalse_When_AccountNotExists()
        {
            //  Arrange
            //  the database has just been emptied so there are no accounts
            const int accountNumber = 1;

            //  Act
            var result = await DataRepositoryClient.AccountExistsAsync(accountNumber);

            //  Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public async Task ReturnTrue_When_AccountExists()
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
            var result = await DataRepositoryClient.AccountExistsAsync(account.AccountNumber);

            //  Assert
            Assert.That(result, Is.True);
        }
    }
}
