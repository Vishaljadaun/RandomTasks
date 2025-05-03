using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RandomTasks.Controllers
{
    [Authorize(Roles = "2")]
    public class CustomerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
