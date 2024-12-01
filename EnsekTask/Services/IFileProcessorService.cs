using EnsekTask.Models.Responses;

namespace EnsekTask.Services;

public interface IFileProcessorService
{
    Task ProcessFileAsync(Stream body, MeterReadingUploadResponse result);
    bool IsFileLineValidFormat(string line);
}