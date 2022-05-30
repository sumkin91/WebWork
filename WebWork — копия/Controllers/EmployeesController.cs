using Microsoft.AspNetCore.Mvc;
using WebWork.Services.Interfaces;
using WebWork.ViewModels;
using WebWork.Models;

namespace WebWork.Controllers;

//[Route("Staff/{action=Index}/{id?}")] // переопределение адреса контроллера
public class EmployeesController : Controller
{
    private IEmployeesData _Employees;

    public EmployeesController(IEmployeesData Employees) { _Employees = Employees; }
    public IActionResult Index()
    {
        var employees = _Employees.GetAll();
        return View(employees);
    }

    //[Route("Staff/Info/{id?}")]// переопределение адреса действия, указывать контроллер необязательно (контроллер по умолчанию)
    //[Route("[controller]/Info/{id?}")] // с указанием контроллера
    public IActionResult Details(int Id)
    {
        var employee = _Employees.GetById(Id);
        if (employee is null)
            return NotFound();

        return View(employee);
    }

    public IActionResult Create() => View(nameof(Edit), new EmployeesViewModel());

    public IActionResult Edit(int? id)
    {
        if(id == null) return View(new EmployeesViewModel());// если id отсутствует

        var employee = _Employees.GetById((int)id);
        if (employee is null)
            return NotFound();

        var view_model = new EmployeesViewModel
        {
            Id = employee.Id,
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            Patronymic = employee.Patronymic,
            Age = employee.Age,
        };
        
        return View(view_model);
    }

    [HttpPost]
    public IActionResult Edit(EmployeesViewModel Model)
    {
        //ручная проверка валидации модели
        if (Model.LastName == "QWE" && Model.FirstName == "QWE" && Model.Patronymic == "QWE")
            ModelState.AddModelError("", "QWE - плохой выбор!");

        if (Model.FirstName == "Asd") ModelState.AddModelError("FirstName", "Asd - плохо!");
        
        //валидация модели c отправкой всех ошибок модели
        if(!ModelState.IsValid) return View(Model);
        
        var employee = new Employee
        {
            Id = Model.Id,
            FirstName = Model.FirstName,
            LastName = Model.LastName,
            Patronymic = Model.Patronymic,
            Age = Model.Age,
        };
        if (Model.Id == 0)
        {
            var new_employee_id = _Employees.Add(employee);
            return RedirectToAction(nameof(Details), new {Id = new_employee_id});
        }
        
        _Employees.Edit(employee);//ввод изменения в сервис

        return RedirectToAction(nameof(Index)); 
    }

    //опреации удаления не реализуются путем GET запроса
    public IActionResult Delete(int id)
    {
        //так делать нельзя, могут взломать путем отправки индексов
        //_Employees.Delete(id);
        //return RedirectToAction(nameof(Index));

        var employee = _Employees.GetById(id);
        if (employee is null)
            return NotFound();

        var view_model = new EmployeesViewModel
        {
            Id = employee.Id,
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            Patronymic = employee.Patronymic,
            Age = employee.Age,
        };

        return View(view_model);
    }

    [HttpPost]
    public IActionResult DeleteConfirmed(int id)
    {
        if (!_Employees.Delete(id)) return NotFound();
        
        return RedirectToAction(nameof(Index));
    }
}
