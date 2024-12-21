using Clarion.Ecom.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Clarion.Ecom.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Dashboard()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetAppConfig()
        {
            var appSettings = new AppSettings();
            appSettings.APIUrl = _configuration["APIUrl"] ?? "";
            if (string.IsNullOrEmpty(appSettings.APIUrl))
            {
                return BadRequest("API host Url not found");
            }
            return Ok(appSettings);
        }

       

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
