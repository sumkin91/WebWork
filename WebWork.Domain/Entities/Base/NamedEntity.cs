using WebWork.Domain.Entities.Base.Interfaces;

namespace WebWork.Domain.Entities.Base
{
    public abstract class NamedEntity : Entity, INamedEntity
    {
        public string Name { get; set; } = null!;
    }
}
