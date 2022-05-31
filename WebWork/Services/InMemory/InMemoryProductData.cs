using WebWork.Services.Interfaces;
using WebWork.Data;
using WebWork.Domain.Entities;
using WebWork.Domain;

namespace WebWork.Services.InMemory
{
    public class InMemoryProductData : IProductData
    {
        public IEnumerable<Section> GetSections() => TestData.Sections;

        public IEnumerable<Brand> GetBrands() => TestData.Brands;

        public IEnumerable<Product> GetProducts(ProductFilter? Filter)
        {
            IEnumerable<Product> query = TestData.Products;

            //if(Filter != null && Filter.SectionId != null)
            //    products = products.Where(x => x.SectionId == Filter.SectionId);

            if (Filter is { SectionId: { } section_id })
                query = query.Where(q => q.SectionId == section_id);

            if (Filter is { BrandId: { } brand_id })
                query = query.Where(q => q.SectionId == brand_id);

            return query;
        }
    }
}
