using WebWork.Domain.Entities;
using WebWork.DAL.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using WebWork.Intefaces.Services;
using Microsoft.Extensions.Logging;

namespace WebWork.Services.Services.InSQL;

public class SqlEmployeeData : IEmployeeData
{
    private readonly WebWorkDB _db;
    private readonly ILogger<SqlEmployeeData> _Logger;

    public SqlEmployeeData(WebWorkDB db, ILogger<SqlEmployeeData> Logger)
    {
        _db = db;
        _Logger = Logger;
    }

    public int Add(Employee employee)
    {
        if (employee is null) throw new ArgumentNullException(nameof(employee));

        //требуется только для хранения данных в памяти, для БД - не требуется
        //if (_db.Employees.Contains(employee)) return employee.Id;

        _db.Employees.Add(employee);

        _db.SaveChanges();// если работа с БД, то вызвать SAveChange() здесь! БД ничего не узнает и идентификатор не будет получен

        _Logger.LogInformation("Сотрудник {0} добавлен", employee); //интерполяцию не использовать

        return employee.Id;
    }

    public bool Delete(int id)
    {
        //var employee = GetById(id);
        var employee = _db.Employees
            .Select(e => new Employee { Id = e.Id }) //только ID достаем
            .FirstOrDefault(e => e.Id == id);
        if (employee is null)
        {
            _Logger.LogWarning("Запись не найдена при попытке удаления сотрудника с id: {0}", id);
            return false;
        }

        _db.Remove(employee);

        _db.SaveChanges();// если работа с БД, то вызвать SAveChange() здесь! БД ничего не узнает и идентификатор не будет получен

        _Logger.LogInformation("Сотрудник {0} удален", employee);

        return true;
    }

    public bool Edit(Employee employee)
    {
        if (employee is null) throw new ArgumentNullException(nameof(employee));

        //требуется только для хранения данных в памяти, для БД - не требуется
        //if (_db.Employees.Contains(employee)) return true;

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

        _db.SaveChanges();// если работа с БД, то вызвать SAveChange() здесь! БД ничего не узнает и идентификатор не будет получен

        _Logger.LogInformation("Сотрудник {0} отредактирован", employee);

        return true;
    }

    public IEnumerable<Employee> Get(int Skip, int Take)
    {
        IQueryable<Employee> query = _db.Employees;
        if (Take == 0) return Enumerable.Empty<Employee>();

        if (Skip > 0) query = query.Skip(Skip);

        return query.Take(Take);
    }

    public IEnumerable<Employee> GetAll()
    {
        return _db.Employees;
    }

    public Employee? GetById(int id)
    {
        //return _db.Employees.FirstOrDefault(e => e.Id == id);
        //return _db.Employees.SingleOrDefault(e => e.Id == id);
        return _db.Employees.Find(id);
    }

    public int GetCount() => _db.Employees.Count();

}
