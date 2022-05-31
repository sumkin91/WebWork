using WebWork.Domain.Entities;
using WebWork.Domain;
namespace WebWork.Services.Interfaces;

public interface IProductData
{
    public IEnumerable<Section> GetSections();
    public IEnumerable<Brand> GetBrands();
    public IEnumerable<Product> GetProducts(ProductFilter? Filter = null);
}
