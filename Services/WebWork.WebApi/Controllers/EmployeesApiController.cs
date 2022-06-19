using Microsoft.AspNetCore.Mvc;
using WebWork.Intefaces.Services;
using WebWork.Domain.Entities;

namespace WebWork.WebApi.Controllers;

[ApiController]
[Route("api/employees")]
public class EmployeesApiController : ControllerBase
{
    private readonly IEmployeeData _EmployeeData;
    private readonly ILogger<EmployeesApiController> _Logger;

    public EmployeesApiController(IEmployeeData EmployeeData, ILogger<EmployeesApiController> Logger)
    {
        _EmployeeData = EmployeeData;
        _Logger = Logger;
    }

    [HttpGet("count")] //GET -> api/employees/count
    public IActionResult GetCount()
    {
        var result = _EmployeeData.GetCount();
        return Ok(result);
    }

    [HttpGet]//GET -> api/employees
    public IActionResult GetAll()
    {
        if (_EmployeeData.GetCount() == 0)
            return NoContent();
        
        var result = _EmployeeData.GetAll();
        return Ok(result);
    }

    [HttpGet("[[{Skip:int}/{Take:int}]]")]//GET -> api/employees[2:4]    [HttpGet("{Skip:int}/{Take:int}")]//GET -> api/employees/2/4)
    public IActionResult Get(int Skip, int Take)
    {
        if(Skip < 0 || Take < 0) return BadRequest();
        
        if (Take ==0 || Skip > _EmployeeData.GetCount())
            return NoContent();

        var result = _EmployeeData.Get(Skip, Take);
        return Ok(result);
    }
    [HttpPost("{Id:int}")]
    public IActionResult GetById(int Id)
    {
        var result = _EmployeeData.GetById(Id);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public IActionResult Add([FromBody]Employee employees)
    {
        var id = _EmployeeData.Add(employees);
        return CreatedAtAction(nameof(GetById), new { Id= id }, employees);
    }

    [HttpPut]
    public IActionResult Edit([FromBody] Employee employees)
    {
        var result = _EmployeeData.Edit(employees);
        if(result)
            return Ok(true);
        return NotFound(false);        
    }

    [HttpDelete("{Id:int}")]
    public IActionResult Delete(int Id)
    {
        var status = _EmployeeData.Delete(Id);
        return status ? Ok() : NotFound();
    }
}
