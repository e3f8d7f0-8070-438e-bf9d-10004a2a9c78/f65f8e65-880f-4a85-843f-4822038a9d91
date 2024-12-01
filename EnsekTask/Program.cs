using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

//  Allow testing of internals
[assembly: InternalsVisibleTo("EnsekTaskTests")]

namespace EnsekTask
{
    //  This is startup code. No need to cover it with tests
    [ExcludeFromCodeCoverage]
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();

            var startup = new Startup(builder.Configuration);
            startup.ConfigureServices(builder.Services);

            var app = builder.Build();

            var loggerFactory = app.Services.GetRequiredService<ILoggerFactory>();
            loggerFactory.AddFile("Logs/app-{Date}.txt");
            var logger = app.Services.GetRequiredService<ILogger<Program>>();
            logger.LogInformation("App starting");

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}");

            await app.RunAsync();
        }
    }
}
