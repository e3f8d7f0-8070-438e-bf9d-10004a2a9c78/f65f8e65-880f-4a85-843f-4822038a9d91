using EnsekTask.Clients;
using EnsekTask.Configuration;
using EnsekTask.Services;
using System.Diagnostics.CodeAnalysis;

namespace EnsekTask
{
    //  This is startup code. No need to cover it with tests
    [ExcludeFromCodeCoverage]
    public class Startup(ConfigurationManager configuration)
    {

        public void ConfigureServices(IServiceCollection services)
        {
            var dataRepositoryConfiguration = configuration.GetSection("dataRepository").Get<DataRepositoryConfiguration>();
            services.AddSingleton<IDataRepositoryConfiguration>(dataRepositoryConfiguration!);

            services.AddSingleton<IFileProcessorService, FileProcessorService>();
            services.AddSingleton<IDataRepositoryClient, DataRepositoryClient>();
        }
    }
}
