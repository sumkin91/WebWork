using WebWork.Domain.Entities.Base;
using WebWork.Domain.Entities.Base.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebWork.Domain.Entities;

[Index(nameof(Name), IsUnique =false)] //имена таблицы могут повторяться из-за связи с родительской секцией
public class Section: NamedEntity, IOrderedEntity
{
    public int Order { get; set; }

    public int? ParentId { get; set; }

    [ForeignKey(nameof(ParentId))]// для ручного создания навигационного ключа
    public Section? Parent { get; set; }//навигационное свойство

    public ICollection<Product> Products { get; set; } = new HashSet<Product>(); //коллекция продуктов
}
