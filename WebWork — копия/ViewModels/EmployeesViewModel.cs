using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebWork.ViewModels;

public class EmployeesViewModel : IValidatableObject
{
    [HiddenInput(DisplayValue = false)]
    [Display(Name = "Идентификатор")]
    public int Id { get; set; }

    [Display(Name = "Фамилия")]
    [Required(ErrorMessage = "Фамилия обязательно!")]
    [StringLength(30, MinimumLength = 2, ErrorMessage ="Длина строки от 2 до 30 символов")]
    [RegularExpression("([А-ЯЁ][а-яё]+)|([A-Z][a-z]+)", ErrorMessage = "Неверный формат! Либо все буквы русские, либо - латинские! Первая буква заглавная!")]
    public string? LastName { get; set; } = null;

    [Display(Name ="Имя")]
    [Required(ErrorMessage = "Фамилия обязательно!")]
    [StringLength(30, MinimumLength = 2, ErrorMessage = "Длина строки от 2 до 30 символов")]
    [RegularExpression("([А-ЯЁ][а-яё]+)|([A-Z][a-z]+)", ErrorMessage = "Неверный формат! Либо все буквы русские, либо - латинские! Первая буква заглавная!")]
    public string? FirstName { get; set; }

    [Display(Name = "Отчество")]
    [StringLength(30, ErrorMessage = "Длина 30 символов")]
    [RegularExpression("([А-ЯЁ][а-яё]+)|([A-Z][a-z]+)", ErrorMessage = "Неверный формат! Либо все буквы русские, либо - латинские! Первая буква заглавная!")]
    public string? Patronymic { get; set; }

    [Display(Name = "Возраст")]
    [Range(10,100,ErrorMessage = "Возраст от 10 до 100 лет")]
    [Required]
    public int Age { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext Context)
    {
        if (LastName == "Abc" && FirstName == "Abc" && Patronymic == "Abc")
        {
            return new[]
            {
                new ValidationResult("Везде - Abc", new[]
                {
                    nameof(LastName),
                    nameof(FirstName),
                    nameof(Patronymic)
                })
            };
        }
        return new[]
            {
                ValidationResult.Success!,
            };
    }
}
