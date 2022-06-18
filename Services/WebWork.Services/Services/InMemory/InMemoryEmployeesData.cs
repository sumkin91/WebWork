using Microsoft.Extensions.Logging;
using WebWork.Data;
//using WebWork.Models.InMemory;
using WebWork.Domain.Entities;
using WebWork.Intefaces.Services;

namespace WebWork.Services.InMemory;

public class InMemoryEmployeesData : IEmployeeData
{
    private readonly ICollection<Employee> _Employees;
    private readonly ILogger<InMemoryEmployeesData> _Logger;
    private int _LastFreeId;

    public InMemoryEmployeesData(ILogger<InMemoryEmployeesData> Logger)
    {
        _Employees = TestData.Employees;
        _Logger = Logger;

        if (_Employees.Count > 0)
        {
            _LastFreeId = _Employees.Max(e => e.Id) + 1;
        }
        else _LastFreeId = 1;
    }

    public IEnumerable<Employee> GetAll()
    {
        return _Employees;
    }

    public Employee? GetById(int id)
    {
        var employee = _Employees.FirstOrDefault(e => e.Id == id);
        return employee;
    }

    public int Add(Employee employee)
    {
        if (employee is null) throw new ArgumentNullException(nameof(employee));

        //требуется только для хранения данных в памяти, для БД - не требуется
        if (_Employees.Contains(employee)) return employee.Id;
        employee.Id = _LastFreeId;
        _LastFreeId++;

        _Employees.Add(employee);

        // если работа с БД, то вызвать SAveChange() здесь! БД ничего не узнает и идентификатор не будет получен

        _Logger.LogInformation("Сотрудник {0} добавлен", employee); //интерполяцию не использовать

        return employee.Id;
    }

    public bool Edit(Employee employee)
    {
        if (employee is null) throw new ArgumentNullException(nameof(employee));

        //требуется только для хранения данных в памяти, для БД - не требуется
        if (_Employees.Contains(employee)) return true;

        var db_employee = GetById(employee.Id);
        if (db_employee is null)
        {
            _Logger.LogWarning("Запись не найдена при попытке редактирования сотрудника {0}", employee);
            return false;
        }

        db_employee.Id = employee.Id;
        db_employee.LastName = employee.LastName;
        db_employee.FirstName = employee.FirstName;
        db_employee.Patronymic = employee.Patronymic;
        db_employee.Age = employee.Age;

        // если работа с БД, то вызвать SAveChange() здесь! БД ничего не узнает и идентификатор не будет получен

        _Logger.LogInformation("Сотрудник {0} отредактирован", employee);

        return true;

    }

    public bool Delete(int id)
    {
        var employee = GetById(id);
        if (employee is null)
        {
            _Logger.LogWarning("Запись не найдена при попытке удаления сотрудника с id: {0}", id);
            return false;
        }

        _Employees.Remove(employee);

        _Logger.LogInformation("Сотрудник {0} удален", employee);

        return true;
    }

    public int GetCount()
    {
        return _Employees.Count;
    }

    public IEnumerable<Employee> Get(int Skip, int Take)
    {
        IEnumerable<Employee> query = _Employees;
        if (Take == 0) return Enumerable.Empty<Employee>();

        if (Skip > 0)
        {
            if (Skip > _Employees.Count()) return Enumerable.Empty<Employee>();
            query = query.Skip(Skip);
        }

        return query.Take(Take);
    }
}
