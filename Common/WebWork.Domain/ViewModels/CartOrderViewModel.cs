﻿
namespace WebWork.ViewModels;

public class CartOrderViewModel
{
    public CartViewModel? Cart { get; set; }

    public OrderViewModel Order { get; set; } = new();
}