//using WebWork.Models.InMemory;
using WebWork.Domain.Entities;

namespace WebWork.Services.Interfaces;

public interface IEmployeeData
{
    int GetCount();
    IEnumerable<Employee> GetAll();
    Employee? GetById(int id);   

    IEnumerable<Employee> Get(int Skip, int Take);

    int Add(Employee employee); 

    bool Edit(Employee employee);

    bool Delete(int id);
}
