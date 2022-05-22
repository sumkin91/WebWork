using Microsoft.AspNetCore.Mvc;
using WebWork.Models;

namespace WebWork.Controllers;

public class HomeController : Controller
{
    public IActionResult Index() => View();

    public IActionResult Greetings(string? id)
    {
        return Content($"Hello from first controller - {id}");
    }

    public IActionResult Contacts() => View();

    public IActionResult Error404() => View();
}
