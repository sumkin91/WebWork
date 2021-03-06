using Microsoft.AspNetCore.Mvc;
using WebWork.Intefaces.Services;
using WebWork.Domain.Entities;
using WebWork.Intefaces;

namespace WebWork.WebApi.Controllers;

/// <summary>Управление сотрудниками</summary>
[ApiController]
//[Route("api/employees")]
[Route(WebApiAddresses.V1.Employees)]
//[Produces("application/json")]//тип контента
//[Produces("application/xml")]
public class EmployeesApiController : ControllerBase
{
    private readonly IEmployeeData _EmployeeData;
    private readonly ILogger<EmployeesApiController> _Logger;

    public EmployeesApiController(IEmployeeData EmployeeData, ILogger<EmployeesApiController> Logger)
    {
        _EmployeeData = EmployeeData;
        _Logger = Logger;
    }

    /// <summary>Количество сотрудников</summary>
    /// <returns></returns>
    [HttpGet("count")] //GET -> api/employees/count
    public IActionResult GetCount()
    {
        var result = _EmployeeData.GetCount();
        return Ok(result);
    }

    /// <summary>Полный список сотрудников</summary>
    [HttpGet]//GET -> api/employees
    public IActionResult GetAll()
    {
        if (_EmployeeData.GetCount() == 0)
            return NoContent();
        
        var result = _EmployeeData.GetAll();
        return Ok(result);
    }

    /// <summary>Фрагмент списка сотрудников</summary>
    /// <param name="Skip">Пропускаемое количество элементов в начале списка</param>
    /// <param name="Take">Количество элементов в выборке</param>
    [HttpGet("[[{Skip:int}/{Take:int}]]")]//GET -> api/employees[2:4]    [HttpGet("{Skip:int}/{Take:int}")]//GET -> api/employees/2/4)
    public IActionResult Get(int Skip, int Take)
    {
        if(Skip < 0 || Take < 0) return BadRequest();
        
        if (Take ==0 || Skip > _EmployeeData.GetCount())
            return NoContent();

        var result = _EmployeeData.Get(Skip, Take);
        return Ok(result);
    }

    /// <summary>Сотрудник с заданным идентификатором</summary>
    /// <param name="Id">Идентификатор сотрудника</param>
    [HttpGet("{Id:int}")]
    public IActionResult GetById(int Id)
    {
        var result = _EmployeeData.GetById(Id);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>Добавление нового сотрудника</summary>
    /// <param name="employees">Добавляемый сотрудник</param>
    [HttpPost]
    public IActionResult Add([FromBody]Employee employees)
    {
        var id = _EmployeeData.Add(employees);
        return CreatedAtAction(nameof(GetById), new { Id= id }, employees);
    }

    /// <summary>Внесение изменений в информацию о сотруднике</summary>
    /// <param name="employees">Структура с новой информацией о сотруднике</param>
    [HttpPut]
    public IActionResult Edit([FromBody] Employee employees)
    {
        var result = _EmployeeData.Edit(employees);
        if(result)
            return Ok(true);
        return NotFound(false);        
    }

    /// <summary>Удаление сотрудника</summary>
    /// <param name="Id">Идентификатор сотрудника</param>
    [HttpDelete("{Id:int}")]
    public IActionResult Delete(int Id)
    {
        var status = _EmployeeData.Delete(Id);
        return status ? Ok() : NotFound();
    }
}
