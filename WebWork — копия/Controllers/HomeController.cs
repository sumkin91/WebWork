using Microsoft.AspNetCore.Mvc;
using WebWork.Models;
using WebWork.Services.Interfaces;
using WebWork.ViewModels;

namespace WebWork.Controllers;

public class HomeController : Controller
{
    public IActionResult Index([FromServices] IProductData ProductData)
    {
        var products = ProductData.GetProducts()
           .OrderBy(p => p.Order)
           .Take(6)
           .Select(p => new ProductViewModel
           {
               Id = p.Id,
               Name = p.Name,
               Price = p.Price,
               ImageUrl = p.ImageUrl,
           });

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
