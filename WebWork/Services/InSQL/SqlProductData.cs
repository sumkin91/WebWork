using WebWork.Domain;
using WebWork.Domain.Entities;
using WebWork.Services.Interfaces;
using WebWork.DAL.Context;

namespace WebWork.Services.InSQL;

public class SqlProductData : IProductData
{
    private readonly WebWorkDB _db;
    
    public SqlProductData(WebWorkDB db)
    {
        _db = db;
    }

    public IEnumerable<Brand> GetBrands() => _db.Brands;//.AsEnumerable();


    public IEnumerable<Product> GetProducts(ProductFilter? Filter = null)
    {
        IQueryable<Product> query = _db.Products;

        if (Filter is { SectionId: { } section_id })
            query = query.Where(q => q.SectionId == section_id);

        if (Filter is { BrandId: { } brand_id })
            query = query.Where(q => q.SectionId == brand_id);

        return query;
    } 


    public IEnumerable<Section> GetSections() => _db.Sections;

}
