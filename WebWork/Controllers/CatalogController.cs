using Microsoft.AspNetCore.Mvc;

namespace WebWork.Controllers;

public class CatalogController : Controller
{
    public IActionResult Index() => View(); // shop.html
}
