using WebWork.Domain.Entities;
using WebWork.Services.Interfaces;
using WebWork.DAL.Context;

namespace WebWork.Services.InSQL;

public class SqlEmployeeData : IEmployeesData
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
        if (_db.Employees.Contains(employee)) return employee.Id;
       
        _db.Employees.Add(employee);

        _db.SaveChanges();// если работа с БД, то вызвать SAveChange() здесь! БД ничего не узнает и идентификатор не будет получен

        _Logger.LogInformation("Сотрудник {0} добавлен", employee); //интерполяцию не использовать

        return employee.Id;
    }

    public bool Delete(int id)
    {
        var employee = GetById(id);
        if (employee is null)
        {
            _Logger.LogWarning("Запись не найдена при попытке удаления сотрудника с id: {0}", id);
            return false;
        }

        _db.Employees.Remove(employee);

        _Logger.LogInformation("Сотрудник {0} удален", employee);

        return true;
    }

    public bool Edit(Employee employee)
    {
        if (employee is null) throw new ArgumentNullException(nameof(employee));

        //требуется только для хранения данных в памяти, для БД - не требуется
        if (_db.Employees.Contains(employee)) return true;

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

        _db.Employees.Add(db_employee);
        _db.SaveChanges();// если работа с БД, то вызвать SAveChange() здесь! БД ничего не узнает и идентификатор не будет получен

        _Logger.LogInformation("Сотрудник {0} отредактирован", employee);

        return true;
    }

    public IEnumerable<Employee> GetAll() 
    {
        return _db.Employees;
    }

    public Employee? GetById(int id)
    {
        return _db.Employees.Where(e => e.Id == id).FirstOrDefault();
    }
}
