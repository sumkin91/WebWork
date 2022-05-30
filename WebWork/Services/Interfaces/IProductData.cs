using WebWork.Domain.Entities;
using WebWork.Domain;
namespace WebWork.Services.Interfaces;

public interface IProductData
{
    IEnumerable<Section> GetSections();
    IEnumerable<Brand> GetBrands();

    IEnumerable<Product> GetProducts(ProductFilter? Filter = null);
}
