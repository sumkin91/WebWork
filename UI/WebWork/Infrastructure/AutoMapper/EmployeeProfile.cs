using AutoMapper;
using WebWork.Domain.Entities;
using WebWork.Domain.ViewModels;

namespace WebWork.Infrastructure.AutoMapper;

public class EmployeeProfile : Profile
{
    public EmployeeProfile()
    {
        CreateMap<Employee, EmployeeViewModel>() //связывание полей
           // .ForMember(m => m.FirstName, o => o.MapFrom(e => e.FirstName)) //связывание полей при отличии
            .ReverseMap(); //для двухсторонней связи (two-way)
    }
}
