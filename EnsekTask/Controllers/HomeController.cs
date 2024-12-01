using EnsekTask.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace EnsekTask.Controllers
{
    public class HomeController(ILogger<HomeController> logger) : Controller
    {
        //  nothing that can be tested here really
        [ExcludeFromCodeCoverage]
        public IActionResult Index()
        {
            logger.LogInformation("Home page has been requested");
            return View();
        }

        //  nothing that can be tested here really
        [ExcludeFromCodeCoverage]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
