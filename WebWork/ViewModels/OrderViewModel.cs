﻿using System.ComponentModel.DataAnnotations;

namespace WebWork.ViewModels;

public class OrderViewModel
{
    [Required, MaxLength(200)]
    [Display(Name = "Адрес заказа")]
    public string? Address { get; set; }

    [Required, MaxLength(200)]
    [Display(Name = "Телефон для связи")]
    [DataType(DataType.PhoneNumber)]
    public string? Phone { get; set; }

    [MaxLength(200)]
    [Display(Name = "Комментарий")]
    [DataType(DataType.MultilineText)]  
    public string? Description { get; set; }
}
