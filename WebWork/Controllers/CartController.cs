using Microsoft.AspNetCore.Mvc;

namespace WebWork.Controllers;

public class CartController : Controller
{
    public IActionResult Index() => View();
}
