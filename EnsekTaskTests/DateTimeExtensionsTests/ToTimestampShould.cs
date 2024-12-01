using EnsekTask.Extensions;
using NUnit.Framework;

namespace EnsekTaskTests.DateTimeExtensionsTests
{
    internal class ToTimestampShould
    {
        [Test]
        public void Convert_DateTime_To_ExpectedTimestampValue()
        {
            //  Arrange
            var when = new DateTime(2025, 1, 1, 12, 24, 17);

            //  Act
            var result = when.ToTimestamp();

            //  Assert
            //  value calculated at https://www.unixtimestamp.com/
            Assert.That(result, Is.EqualTo(1735734257));
        }
    }
}
