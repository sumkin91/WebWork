using WebWork.Domain.Entities.Base;
using WebWork.Domain.Entities.Base.Interfaces;

namespace WebWork.Domain.Entities;

public class Product : NamedEntity, IOrderedEntity
{
    public int Order {get; set;}

    public int SectionId {get; set;}

    public int BrandId { get; set; }
    
    public string? ImageUrl {get; set;}

    public decimal Price {get; set;}
}
