using EnsekTask.Models.Entities;
using Moq;
using NUnit.Framework;
using System.Text.Json;

namespace EnsekTaskTests.FileProcessorServiceTests
{
    internal class ProcessFileLineAsyncShould : FileProcessorServiceTestsBase
    {
        [Test]
        public async Task Return_False_When_DuplicateExists()
        {
            //  Arrange
            DataRepositoryClientMock
                .SetupSequence(q => q.AddMeterReadingAsync(It.Is<MeterReading>(
                    w => JsonSerializer.Serialize(MeterReadingDayOne, (JsonSerializerOptions?)null)
                        .Equals(JsonSerializer.Serialize(w, (JsonSerializerOptions?)null)))))
                .ReturnsAsync(true)
                .ReturnsAsync(false);

            //  Act
            var resultFirst = await FileProcessorService.ProcessFileLineAsync(LineDayOne);
            var resultSecond = await FileProcessorService.ProcessFileLineAsync(LineDayOne);

            //  Assert
            Assert.That(resultFirst, Is.True);
            Assert.That(resultSecond, Is.False);
        }

        [Test]
        public async Task Return_False_When_LaterEntryExists()
        {
            //  Arrange
            DataRepositoryClientMock
                .Setup(q => q.AddMeterReadingAsync(It.Is<MeterReading>(
                    w => JsonSerializer.Serialize(MeterReadingDayTwo, (JsonSerializerOptions?)null)
                        .Equals(JsonSerializer.Serialize(w, (JsonSerializerOptions?)null)))))
                .ReturnsAsync(true);
            DataRepositoryClientMock
                .Setup(q => q.AddMeterReadingAsync(It.Is<MeterReading>(
                    w => JsonSerializer.Serialize(MeterReadingDayOne, (JsonSerializerOptions?)null)
                        .Equals(JsonSerializer.Serialize(w, (JsonSerializerOptions?)null)))))
                .ReturnsAsync(false);

            //  Act
            var resultFirst = await FileProcessorService.ProcessFileLineAsync(LineDayTwo);
            var resultSecond = await FileProcessorService.ProcessFileLineAsync(LineDayOne);

            //  Assert
            Assert.That(resultFirst, Is.True);
            Assert.That(resultSecond, Is.False);
        }

        [Test]
        public async Task Return_True_When_NoDuplicateExists()
        {
            //  Arrange
            DataRepositoryClientMock
                .Setup(q => q.AddMeterReadingAsync(It.Is<MeterReading>(
                    w => JsonSerializer.Serialize(MeterReadingDayOne, (JsonSerializerOptions?)null)
                        .Equals(JsonSerializer.Serialize(w, (JsonSerializerOptions?)null)))))
                .ReturnsAsync(true);

            //  Act
            var result = await FileProcessorService.ProcessFileLineAsync(LineDayOne);

            //  Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public async Task Return_True_When_NoLaterEntryExists()
        {
            //  Arrange
            DataRepositoryClientMock
                .Setup(q => q.AddMeterReadingAsync(It.Is<MeterReading>(
                    w => JsonSerializer.Serialize(MeterReadingDayOne, (JsonSerializerOptions?)null)
                        .Equals(JsonSerializer.Serialize(w, (JsonSerializerOptions?)null)))))
                .ReturnsAsync(true);
            DataRepositoryClientMock
                .Setup(q => q.AddMeterReadingAsync(It.Is<MeterReading>(
                    w => JsonSerializer.Serialize(MeterReadingDayTwo, (JsonSerializerOptions?)null)
                        .Equals(JsonSerializer.Serialize(w, (JsonSerializerOptions?)null)))))
                .ReturnsAsync(true);

            //  Act
            var resultFirst = await FileProcessorService.ProcessFileLineAsync(LineDayOne);
            var resultSecond = await FileProcessorService.ProcessFileLineAsync(LineDayTwo);

            //  Assert
            Assert.That(resultFirst, Is.True);
            Assert.That(resultSecond, Is.True);
        }
    }
}
