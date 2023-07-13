using Client2.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Client2.Controllers
{
    public class LatihanChart : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public LatihanChart(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}