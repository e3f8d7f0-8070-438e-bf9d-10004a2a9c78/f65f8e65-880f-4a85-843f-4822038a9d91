using NUnit.Framework;

namespace EnsekTaskTests.FileProcessorServiceTests
{
    public class ValidateFileLineFormatShould : FileProcessorServiceTestsBase
    {
        [Test]
        //  allow entire line formatted correctly
        [TestCase("1,22/04/2019 09:24,00000", true)]
        //  allow extra empty columns at end of row
        [TestCase("1,22/04/2019 09:24,00000,", true)]
        [TestCase("1,22/04/2019 09:24,00000,,,,,,,,,", true)]
        //  disallow extra non-empty columns
        [TestCase("1,22/04/2019 09:24,00000,X", false)]
        //  disallow missing data in columns
        [TestCase(",22/04/2019 09:24,00000", false)]
        [TestCase("1,,00000", false)]
        [TestCase("1,22/04/2019 09:24", false)]
        [TestCase("1,22/04/2019 09:24,", false)]
        //  disallow extra, empty columns between the expected ones
        [TestCase("1,,22/04/2019 09:24,00000", false)]
        [TestCase("1,22/04/2019 09:24,,00000", false)]
        //  disallow extra empty column at start of row
        [TestCase(",1,22/04/2019 09:24,00000", false)]
        //  disallow meter reading values in the wrong format
        [TestCase("1,22/04/2019 09:24,0000,", false)]
        [TestCase("1,22/04/2019 09:24,-1234,", false)]
        [TestCase("1,22/04/2019 09:24,-12345,", false)]
        [TestCase("1,22/04/2019 09:24,A1234,", false)]
        [TestCase("1,22/04/2019 09:24,1234A,", false)]
        [TestCase("1,22/04/2019 09:24,123456,", false)]
        //  disallow unrecognised date formats
        [TestCase("1,22,00000,", false)]
        //  disallow impossible dates
        [TestCase("1,29/02/2019 09:24,00000", false)]
        //  disallow future dates
        [TestCase("1,01/01/2036 09:24,00000", false)]
        //  disallow account numbers in the wrong format
        [TestCase("A,22/04/2019 09:24,00000", false)]
        [TestCase("-1,22/04/2019 09:24,00000", false)]
        [TestCase("1.0,22/04/2019 09:24,00000", false)]
        [TestCase("1.1,22/04/2019 09:24,00000", false)]
        public void ReturnExpectedResult_When_ValidatingLineFormat(string line, bool expectedResult)
        {
            //  Arrange

            //  Act
            var result = FileProcessorService.IsFileLineValidFormat(line);

            //  Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }
    }
}
