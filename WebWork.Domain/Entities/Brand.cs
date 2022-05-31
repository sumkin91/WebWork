using WebWork.Domain.Entities.Base;
using WebWork.Domain.Entities.Base.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebWork.Domain.Entities;

//[Table("ProductBrand")]
[Index(nameof(Name), IsUnique = true)]
public class Brand: NamedEntity, IOrderedEntity
{
   // [Column("BrandOrder")]
    public int Order { get; set; }

    public ICollection<Product> Products { get; set; } = new HashSet<Product>();//для бренд связан с товарами в отношении один ко многим, для исключения одних и тех же объектов используется хэш-таблицы (илжно и список)
}
