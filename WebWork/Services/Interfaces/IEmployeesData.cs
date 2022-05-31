//using WebWork.Models.InMemory;
using WebWork.Domain.Entities;

namespace WebWork.Services.Interfaces;

public interface IEmployeesData
{
    public IEnumerable<Employee> GetAll();
    public Employee? GetById(int id);   

    public int Add(Employee employee); 

    public bool Edit(Employee employee);

    public bool Delete(int id);
}
