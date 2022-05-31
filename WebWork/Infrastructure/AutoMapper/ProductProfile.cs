using AutoMapper;
using WebWork.ViewModels;
using WebWork.Domain.Entities;

namespace WebWork.Infrastructure.AutoMapper;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<Product, ProductViewModel>();
    }
}
