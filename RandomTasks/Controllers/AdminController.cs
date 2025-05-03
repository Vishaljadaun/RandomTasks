using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RandomTasks.Controllers
{
    [Authorize(Roles = "1")]
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            // Simulating numbers to show in the dashboard
            ViewBag.TotalUsers = 150;
            ViewBag.TotalOrders = 75;
            ViewBag.TotalRevenue = 12000;

            return View();
        }
    }

}
