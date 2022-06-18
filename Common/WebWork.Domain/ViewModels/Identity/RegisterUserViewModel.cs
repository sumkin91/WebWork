using System.ComponentModel.DataAnnotations;

namespace WebWork.Domain.ViewModels.Identity;

public class RegisterUserViewModel
{
    [Required(ErrorMessage = "Имя является обязательным")]
    [Display(Name = "Имя пользователя")]
    [MaxLength(255)]
    public string? UserName { get; set; }

    [Required(ErrorMessage = "Пароль является обязательным")]
    [Display(Name = "Пароль")]
    [DataType(DataType.Password)]
    [MaxLength(255)]
    public string? Password { get; set; }

    [Required(ErrorMessage = "Не ведено подтверждение пароля")]
    [Display(Name = "Подтверждение пароля")]
    [DataType(DataType.Password)]
    [Compare(nameof(Password), ErrorMessage = "пароль и подтверждение не совпадают!")]
    [MaxLength(255)]
    public string? PasswordConfirm { get; set; }
}
