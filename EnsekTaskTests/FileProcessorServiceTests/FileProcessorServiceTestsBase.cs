using EnsekTask.Clients;
using EnsekTask.Models.Entities;
using EnsekTask.Services;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace EnsekTaskTests.FileProcessorServiceTests
{
    public class FileProcessorServiceTestsBase
    {
        protected Mock<ILogger<FileProcessorService>> LoggerFileProcessorServiceMock = null!;
        protected Mock<IDataRepositoryClient> DataRepositoryClientMock = null!;
        protected FileProcessorService FileProcessorService = null!;
        protected MeterReading MeterReadingDayOne = null!;
        protected MeterReading MeterReadingDayTwo = null!;
        protected string LineDayOne = null!;
        protected string LineDayTwo = null!;

        [SetUp]
        public void SetUp()
        {
            DataRepositoryClientMock = new Mock<IDataRepositoryClient>();
            LoggerFileProcessorServiceMock = new Mock<ILogger<FileProcessorService>>();
            FileProcessorService = new FileProcessorService(LoggerFileProcessorServiceMock.Object,
                DataRepositoryClientMock.Object);

            MeterReadingDayOne = new MeterReading
            {
                AccountNumber = 1,
                Date = new DateTime(2000, 1, 1),
                Reading = 12345
            };
            LineDayOne = $"{MeterReadingDayOne.AccountNumber},{MeterReadingDayOne.Date},{MeterReadingDayOne.Reading}";
            MeterReadingDayTwo = new MeterReading
            {
                AccountNumber = 1,
                Date = new DateTime(2000, 1, 2),
                Reading = 12345
            };
            LineDayTwo = $"{MeterReadingDayTwo.AccountNumber},{MeterReadingDayTwo.Date},{MeterReadingDayTwo.Reading}";
        }

    }
}
