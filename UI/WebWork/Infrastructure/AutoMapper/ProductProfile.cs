using AutoMapper;
using WebWork.Domain.Entities;
using WebWork.Domain.ViewModels;

namespace WebWork.Infrastructure.AutoMapper;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<Product, ProductViewModel>();
    }
}
