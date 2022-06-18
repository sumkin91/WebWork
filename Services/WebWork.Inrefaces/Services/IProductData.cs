using WebWork.Domain.Entities;
using WebWork.Domain;
namespace WebWork.Services.Interfaces;

public interface IProductData
{
    IEnumerable<Section> GetSections();

    Section? GetSectionById(int Id);

    Brand? GetBrandById(int Id);

    IEnumerable<Brand> GetBrands();

    IEnumerable<Product> GetProducts(ProductFilter? Filter = null);

    Product? GetProductById(int Id);
}
