using System.Diagnostics.CodeAnalysis;
//using WebWork.Models.InMemory;
using WebWork.Domain.Entities;
using WebWork.ViewModels;

namespace WebWork.Infrastructure.Mapping;

public static class EmployeeMapper
{
    [return: NotNullIfNotNull("employee")]
    public static EmployeesViewModel? ToView(this Employee? employee) => employee is null
        ? null
        : new EmployeesViewModel
        {
            Id = employee.Id,
            LastName = employee.LastName,
            FirstName = employee.FirstName,
            Patronymic = employee.Patronymic,
            Age = employee.Age,
        };

    [return: NotNullIfNotNull("employee")]
    public static Employee? FromView(this EmployeesViewModel? employee) => employee is null
        ? null
        : new Employee
        {
            Id = employee.Id,
            LastName = employee.LastName,
            FirstName = employee.FirstName,
            Patronymic = employee.Patronymic,
            Age = employee.Age,
        };

    public static IEnumerable<EmployeesViewModel?> ToView(this IEnumerable<Employee?> employees) => employees.Select(ToView);

    public static IEnumerable<Employee?> FromView(this IEnumerable<EmployeesViewModel?> employees) => employees.Select(FromView);
}