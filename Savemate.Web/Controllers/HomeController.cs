using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Savemate.Application.Interface;
using Savemate.Application.Services.IService;
using Savemate.Infrastructure;
using Savemate.Web.Models;
using System.Diagnostics;

namespace Savemate.Web.Controllers
{
    public class HomeController(ILogger<HomeController> logger, UserManager<ApplicationUser> userManager, IDashboardService dashboardService) : Controller
    {
        private readonly ILogger<HomeController> _logger = logger;
        private readonly IDashboardService _dashboardService = dashboardService;
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        public async Task<IActionResult> Index()
        {
            if (!User?.Identity?.IsAuthenticated ?? false)
                return RedirectToAction("Login", "Authentication");

            var userId = _userManager.GetUserId(User!);

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Authentication");
            }
            var dashboard = await _dashboardService.GetDashboardDataAsync(userId) ; 

            return View("Dashboard", dashboard);
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
