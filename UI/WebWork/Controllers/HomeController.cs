using Microsoft.AspNetCore.Mvc;
using WebWork.Infrastructure.Mapping;
using WebWork.Intefaces.Services;

namespace WebWork.Controllers;

public class HomeController : Controller
{
    public IActionResult Index([FromServices] IProductData ProductData)
    {
        var products = ProductData.GetProducts().OrderBy(p => p.Order).Take(6).ToView(); //mapper


        ViewBag.Products = products;

        return View();
    }


    public IActionResult Greetings(string? id)
    {
        return Content($"Hello from first controller - {id}");
    }

    public IActionResult Contacts() => View();

    public IActionResult Error404() => View();
}
