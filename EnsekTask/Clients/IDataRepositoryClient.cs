using EnsekTask.Models.Entities;

namespace EnsekTask.Clients;

public interface IDataRepositoryClient
{
    Task<bool> AddMeterReadingAsync(MeterReading reading);
}