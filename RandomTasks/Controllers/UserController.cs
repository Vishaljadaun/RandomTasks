using Microsoft.AspNetCore.Mvc;
using RandomTasks.Models;

public class UserController : Controller
{
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(UserViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        return RedirectToAction("Success");
    }

    public IActionResult Success()
    {
        return View();
    }
}
