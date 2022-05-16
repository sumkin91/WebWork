using Microsoft.AspNetCore.Mvc;
using WebWork.Models;

namespace WebWork.Controllers;

public class HomeController : Controller
{

    private static readonly List<Employee> __Employees = new()
    {
        new Employee { Id = 1, LastName = "Иванов", FirstName = "Иван", Patronymic = "Иванович", Age = 23 },
        new Employee { Id = 2, LastName = "Петров", FirstName = "Пётр", Patronymic = "Петрович", Age = 27 },
        new Employee { Id = 3, LastName = "Сидоров", FirstName = "Сидор", Patronymic = "Сидорович", Age = 18 },
    };
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Greatings(string? id)
    {
        return Content($"Hello from controller - greatings, id: {id}");
    }

    public IActionResult Employees()
    {
        return View(__Employees);
    }

    public IActionResult EmployeDetails(int id)
    {
        var employee = __Employees.FirstOrDefault(x => x.Id == id);
        if(employee == null) return NotFound();
        return View(employee);
    }
}

