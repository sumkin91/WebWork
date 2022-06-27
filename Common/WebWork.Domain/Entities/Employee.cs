using WebWork.Domain.Entities.Base;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace WebWork.Domain.Entities
{
    /// <summary>Сотрудник</summary>
    [Index(nameof(FirstName), nameof(LastName), nameof(Patronymic), nameof(Age), IsUnique = true)]
    public class Employee : Entity
    {
        /// <summary>Имя</summary>
        [Required]
        public string? FirstName { get; set; }

        /// <summary>Фамилия</summary>
        [Required]
        public string? LastName { get; set; }

        /// <summary>Отчество</summary>
        public string? Patronymic { get; set; }

        /// <summary>Возраст</summary>
        public int Age { get; set; }

        public override string ToString() => $"(id:{Id}){LastName} {FirstName} {Patronymic} - age:{Age}";
    }
}
