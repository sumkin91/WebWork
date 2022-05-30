using WebWork.Domain.Entities.Base;
using WebWork.Domain.Entities.Base.Interfaces;

namespace WebWork.Domain.Entities;

public class Brand: NamedEntity, IOrderedEntity
{
    public int Order { get; set; }
}
