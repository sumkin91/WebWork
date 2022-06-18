using WebWork.Domain;
using WebWork.Domain.Entities;
using WebWork.DAL.Context;
using Microsoft.EntityFrameworkCore;
using WebWork.Intefaces.Services;

namespace WebWork.Services.InSQL;

public class SqlProductData : IProductData
{
    private readonly WebWorkDB _db;
    
    public SqlProductData(WebWorkDB db)
    {
        _db = db;
    }

    public IEnumerable<Brand> GetBrands() => _db.Brands;//.AsEnumerable();

    public Section? GetSectionById(int Id) => _db.Sections
        .Include(s => s.Products)
        .FirstOrDefault(s => s.Id == Id);

    public Brand? GetBrandById(int Id) => _db.Brands
        .Include(b => b.Products)
        .FirstOrDefault(s => s.Id == Id);


    public IEnumerable<Product> GetProducts(ProductFilter? Filter = null)
    {
        IQueryable<Product> query = _db.Products
            .Include(s => s.Section)
            .Include(b => b.Brand);

        if(Filter is { Ids: {Length: > 0 } ids }) 
        { //запрос по ids
            query = query.Where(p => ids.Contains(p.Id));
        }
        else
        {//запрос как есть если нет ids
            if (Filter is { SectionId: { } section_id })
                query = query.Where(q => q.SectionId == section_id);

            if (Filter is { BrandId: { } brand_id })
                query = query.Where(q => q.SectionId == brand_id);
        }

        return query;
    } 


    public IEnumerable<Section> GetSections() => _db.Sections;

    public Product? GetProductById(int Id) => _db.Products
        .Include(s => s.Section)
        .Include(b => b.Brand)
        .FirstOrDefault(p => p.Id == Id);

}
