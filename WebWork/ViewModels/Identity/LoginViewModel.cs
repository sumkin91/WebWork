using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace WebWork.ViewModels.Identity;

public class LoginViewModel
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

    [Display(Name ="Запомнить меня?")]
    public bool RememberMe { get; set; }

    [HiddenInput(DisplayValue =false)]
    public string? ReturnUrl { get; set; }
}
