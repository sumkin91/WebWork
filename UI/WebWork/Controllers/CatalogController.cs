﻿using Microsoft.AspNetCore.Mvc;
using WebWork.Services.Interfaces;
using WebWork.Domain;
using WebWork.ViewModels;
//using WebWork.Infrastructure.Mapping; //ручной маппер
using AutoMapper;
using WebWork.Infrastructure.Mapping;

namespace WebWork.Controllers;

public class CatalogController : Controller
{
    private readonly IProductData _ProductData;

    private readonly IMapper _Mapper;

    public CatalogController(IProductData ProductData, IMapper Mapper)
    {
        _ProductData = ProductData;
        _Mapper = Mapper;
    }

    public IActionResult Index([Bind("BrandId","SectionId")]ProductFilter filter ) // shop.html
    {
        var products = _ProductData.GetProducts(filter);

        return View(new CatalogViewModel
        {
            BrandId = filter.BrandId,
            SectionId = filter.SectionId,
            Products = products.OrderBy(p => p.Order).Select(p => _Mapper.Map<ProductViewModel>(p)),
            //Products = products.OrderBy(p => p.Order).ToView()!, //mapper ручной: Infrastructuries/Mapping
        }); ; 
    }

    public IActionResult Details(int Id)
    {
        var product =_ProductData.GetProductById(Id);
        if (product is null) return NotFound();

        return View(product.ToView());
    }
}
