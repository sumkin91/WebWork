using WebWork.Domain.Entities.Base;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace WebWork.Domain.Entities
{
    [Index(nameof(FirstName), nameof(LastName), nameof(Patronymic), nameof(Age), IsUnique = true)]
    public class Employee : Entity
    {
        [Required]
        public string? FirstName { get; set; }

        [Required]
        public string? LastName { get; set; }

        public string? Patronymic { get; set; }

        public int Age { get; set; }

        public override string ToString() => $"(id:{Id}){LastName} {FirstName} {Patronymic} - age:{Age}";
    }
}
