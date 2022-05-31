using AutoMapper;
using WebWork.ViewModels;
using WebWork.Domain.Entities;

namespace WebWork.Infrastructure.AutoMapper;

public class EmployeesProfile : Profile
{
    public EmployeesProfile()
    {
        CreateMap<Employee, EmployeesViewModel>() //связывание полей
           // .ForMember(m => m.FirstName, o => o.MapFrom(e => e.FirstName)) //связывание полей при отличии
            .ReverseMap(); //для двухсторонней связи (two-way)
    }
}
