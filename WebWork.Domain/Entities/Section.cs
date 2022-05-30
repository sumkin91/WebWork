using WebWork.Domain.Entities.Base;
using WebWork.Domain.Entities.Base.Interfaces;

namespace WebWork.Domain.Entities;

public class Section: NamedEntity, IOrderedEntity
{
    public int Order { get; set; }

    public int? ParentId { get; set; }
}
