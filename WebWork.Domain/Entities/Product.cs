using WebWork.Domain.Entities.Base;
using WebWork.Domain.Entities.Base.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebWork.Domain.Entities;

[Index(nameof(Name), IsUnique = false)]
public class Product : NamedEntity, IOrderedEntity
{
    public int Order {get; set;}

    public int SectionId {get; set;}

    [ForeignKey(nameof(SectionId))]
    [Required]//можно не указывать, если внешний ключ не поддерживает значение null (SectionId без null)
    public Section? Section { get; set; }//навигационное свойство

    public int? BrandId { get; set; }

    [ForeignKey(nameof(BrandId))]
    public Brand? Brand { get; set; }//навигационное свойство

    [Required]
    public string? ImageUrl {get; set;}

    [Column(TypeName = "decimal(18,2)")] // 2 знака после запятой
    public decimal Price {get; set;}
}
