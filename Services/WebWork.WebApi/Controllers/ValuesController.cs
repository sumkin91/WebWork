using Microsoft.AspNetCore.Mvc;

namespace WebWork.WebApi.Controllers;

[ApiController]
//[Route("api/[controller]")]
[Route("api/values")]
public class ValuesController : ControllerBase
{
    private const int __ValueCount = 10;
    
    private static readonly Dictionary<int, string> __Values = Enumerable.Range(1, __ValueCount)
        .Select(i => (Id: i, Value: $"Value-{i}"))
        .ToDictionary(v=> v.Id, v => v.Value);

    private static int __LastFreeId = __ValueCount + 1;

    private readonly ILogger<ValuesController> _Logger;

    public ValuesController(ILogger<ValuesController> Logger)
    {
        _Logger = Logger;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        if(__Values.Count==0)
            return NoContent();
        
        var values = __Values.Values;
        return Ok(values);
    }

    [HttpGet("{Id:int}")]//GET -> api/values/5
    public IActionResult GetById(int Id)
    {
        if(__Values.TryGetValue(Id, out var value))
            return Ok(value);
        return NotFound(new {Id});
    }

    [HttpPost] //POST -> api/values?Value=qwerty ;; api/values || Body:Value=qwerty
    [HttpPost("{Value}")]//POST -> api/values/qwerty
    public IActionResult Add(/*[FromBody]*/string Value)//строго в теле
    {
        var id = __LastFreeId;
        __Values[id] = Value;

        _Logger.LogInformation("Значение {0} добавлено под id:{1}", Value, id);

        __LastFreeId++;

        return CreatedAtAction(nameof(GetById), new {Id = id}, Value);
    }

    [HttpPut("{Id:int}")]
    public IActionResult Edit(int Id, [FromBody] string Value)
    {
        if(!__Values.ContainsKey(Id))
        {
            _Logger.LogWarning("Запись с id: {0} - не найдена", Id);
            return NotFound(new { Id });
        }

        var old_value = __Values[Id];
        __Values[Id] = Value;
        _Logger.LogInformation("Редактирование записи с id:{0} - новое значение {1}", Id, Value);
        return Ok(new {Id, OldValue = old_value, NewValue = Value});
    }

    [HttpDelete("{Id:int}")]
    public IActionResult Delete(int Id)
    {
        if (!__Values.ContainsKey(Id))
        {
            _Logger.LogWarning("Запись с id: {0} - не найдена", Id);
            return NotFound(new { Id });
        }

        var value = __Values[Id];
        __Values.Remove(Id);

        _Logger.LogInformation("Удаление записи с id:{0} - значение {1}", Id, value);

        return Ok(new{ Id, value = value});
    }
}
