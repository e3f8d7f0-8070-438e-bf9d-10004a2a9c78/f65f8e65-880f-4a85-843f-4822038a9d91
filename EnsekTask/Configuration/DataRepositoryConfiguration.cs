namespace EnsekTask.Configuration
{
    public class DataRepositoryConfiguration : IDataRepositoryConfiguration
    {
        public string ConnectionString { get; set; } = null!;
    }
}
