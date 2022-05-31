using WebWork.Domain.Entities.Base;
using System.ComponentModel.DataAnnotations;

namespace WebWork.Domain.Entities
{
    public class Employee : Entity
    {
        [Required]
        public string? FirstName { get; set; }

        [Required]
        public string? LastName { get; set; }

        public string? Patronymic { get; set; }
        [Required]
        public int Age { get; set; }
    }
}
