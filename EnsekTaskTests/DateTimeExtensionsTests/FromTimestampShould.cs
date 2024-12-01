using EnsekTask.Extensions;
using NUnit.Framework;

namespace EnsekTaskTests.DateTimeExtensionsTests
{
    internal class FromTimestampShould
    {
        [Test]
        public void Convert_Timestamp_To_ExpectedDateTimeValue()
        {
            //  Arrange
            var timestamp = 1735734257;
            var expectedResult = new DateTime(2025, 1, 1, 12, 24, 17);

            //  Act
            var result = timestamp.FromTimestamp();

            //  Assert
            //  value calculated at https://www.unixtimestamp.com/
            Assert.That(result, Is.EqualTo(expectedResult));
        }
    }
}
