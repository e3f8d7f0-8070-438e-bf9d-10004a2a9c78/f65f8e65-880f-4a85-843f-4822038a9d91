using EnsekTask.Models.Entities;
using EnsekTask.Models.Responses;
using Moq;
using NUnit.Framework;
using System.Text;
using System.Text.Json;

namespace EnsekTaskTests.FileProcessorServiceTests
{
    public class ProcessFileAsyncShould : FileProcessorServiceTestsBase
    {
        [Test]
        public async Task Return_CorrectSuccessAndFailureCounts_When_DataRepositoryClient_AcceptsAllLines()
        {
            //  Arrange
            var result = new MeterReadingUploadResponse();
            var csv = $"HeaderRow\r\n{LineDayOne}\r\n{LineDayTwo}";
            var csvBytes = Encoding.UTF8.GetBytes(csv);
            var body = new MemoryStream(csvBytes);
            body.Position = 0;

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
            await FileProcessorService.ProcessFileAsync(body, result);

            //  Assert
            Assert.That(result.SuccessCount, Is.EqualTo(2));
            Assert.That(result.FailureCount, Is.EqualTo(0));
        }

        [Test]
        public async Task Return_CorrectSuccessAndFailureCounts_When_DataRepositoryClient_AcceptsAllLines_And_OneIsEmpty()
        {
            //  Arrange
            var result = new MeterReadingUploadResponse();
            var csv = $"HeaderRow\r\n{LineDayOne}\r\n\r\n{LineDayTwo}";
            var csvBytes = Encoding.UTF8.GetBytes(csv);
            var body = new MemoryStream(csvBytes);
            body.Position = 0;

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
            await FileProcessorService.ProcessFileAsync(body, result);

            //  Assert
            Assert.That(result.SuccessCount, Is.EqualTo(2));
            Assert.That(result.FailureCount, Is.EqualTo(0));
        }

        [Test]
        public async Task Return_CorrectSuccessAndFailureCounts_When_DataRepositoryClient_RejectsOneLine()
        {
            //  Arrange
            var result = new MeterReadingUploadResponse();
            var csv = $"HeaderRow\r\n{LineDayTwo}\r\n{LineDayOne}";
            var csvBytes = Encoding.UTF8.GetBytes(csv);
            var body = new MemoryStream(csvBytes);
            body.Position = 0;

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
            await FileProcessorService.ProcessFileAsync(body, result);

            //  Assert
            Assert.That(result.SuccessCount, Is.EqualTo(1));
            Assert.That(result.FailureCount, Is.EqualTo(1));
        }

        [Test]
        public async Task Return_CorrectSuccessAndFailureCounts_When_LineIsBadlyFormed()
        {
            //  Arrange
            var result = new MeterReadingUploadResponse();
            var csv = $"HeaderRow\r\n{LineDayOne},X";
            var csvBytes = Encoding.UTF8.GetBytes(csv);
            var body = new MemoryStream(csvBytes);
            body.Position = 0;

            //  Act
            await FileProcessorService.ProcessFileAsync(body, result);

            //  Assert
            Assert.That(result.SuccessCount, Is.EqualTo(0));
            Assert.That(result.FailureCount, Is.EqualTo(1));
        }
    }
}
