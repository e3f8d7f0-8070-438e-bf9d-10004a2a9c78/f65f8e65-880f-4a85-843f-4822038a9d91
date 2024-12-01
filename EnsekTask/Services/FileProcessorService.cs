using EnsekTask.Clients;
using EnsekTask.Models.Entities;
using EnsekTask.Models.Responses;

namespace EnsekTask.Services
{
    public class FileProcessorService(ILogger<FileProcessorService> logger, IDataRepositoryClient dataRepositoryClient) : IFileProcessorService
    {
        public async Task ProcessFileAsync(Stream body, MeterReadingUploadResponse result)
        {
            var reader = new StreamReader(body);
            var firstRow = true;
            while (true)
            {
                var line = await reader.ReadLineAsync();
                if (line == null)
                {
                    //  end of the stream
                    break;
                }

                if (string.IsNullOrWhiteSpace(line))
                {
                    //  empty line, move on to the next one
                    continue;
                }

                logger.LogInformation(line);

                if (IsFileLineValidFormat(line))
                {
                    if (await ProcessFileLineAsync(line))
                    {
                        result.SuccessCount++;
                    }
                    else
                    {
                        logger.LogWarning("Line was rejected");
                        result.FailureCount++;
                    }
                }
                else if (!firstRow)
                {
                    logger.LogWarning("Invalid line detected in uploaded file");
                    result.FailureCount++;
                }
                firstRow = false;
            }
        }

        public bool IsFileLineValidFormat(string line)
        {
            //  contains at least 3 columns?
            var lineSplits = line.Split(',');
            if (lineSplits.Length < 3)
            {
                return false;
            }

            //  contains exactly three non-empty columns?
            lineSplits = line.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            if (lineSplits.Length != 3)
            {
                return false;
            }

            //  does not contain an empty column at the start?
            lineSplits = line.Split(',', StringSplitOptions.TrimEntries);
            if (lineSplits[0] == string.Empty)
            {
                return false;
            }

            //  we now definitely have an array of strings (columns)
            //  in which the first three contain data

            //  first column is an integer?
            var canParseAccountNumber = int.TryParse(lineSplits[0], out var accountNumber);
            if (!canParseAccountNumber)
            {
                return false;
            }

            //  account number is non-zero and positive?
            if (accountNumber <= 0)
            {
                return false;
            }

            //  second column is in a valid date format?
            var canParseDate = DateTime.TryParse(lineSplits[1], out var date);
            if (!canParseDate)
            {
                return false;
            }

            //  date is today or earlier?
            //  making the (possibly incorrect) assumption that dates/times are UTC
            var tomorrow = DateTime.UtcNow.Date.AddDays(1);
            if (date >= tomorrow)
            {
                return false;
            }

            //  third column is 5 characters long?
            if (lineSplits[2].Length != 5)
            {
                return false;
            }

            //  third column is an integer?
            var canParseMeterReading = int.TryParse(lineSplits[2], out var meterReading);
            if (!canParseMeterReading)
            {
                return false;
            }

            //  meter reading is not negative?
            if (meterReading < 0)
            {
                return false;
            }

            //  if all the checks above have passed,
            //  the line appears to be in a valid format
            return true;
        }

        public async Task<bool> ProcessFileLineAsync(string line)
        {
            //  we have already validated the line so it is safe to assume the values are present
            var lineSplits = line.Split(',', StringSplitOptions.TrimEntries);
            var accountNumber = int.Parse(lineSplits[0]);
            var date = DateTime.Parse(lineSplits[1]);
            var meterReading = int.Parse(lineSplits[2]);
            var reading = new MeterReading
            {
                AccountNumber = accountNumber,
                Date = date,
                Reading = meterReading
            };

            return await dataRepositoryClient.AddMeterReadingAsync(reading);
        }
    }
}
