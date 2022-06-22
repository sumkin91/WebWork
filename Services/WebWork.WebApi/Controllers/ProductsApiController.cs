using Microsoft.AspNetCore.Mvc;
using WebWork.Domain;
using WebWork.Domain.DTO;
using WebWork.Intefaces.Services;

namespace WebWork.WebApi.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsApiController: ControllerBase
{
    private readonly IProductData _ProductData;
    private readonly ILogger<ProductsApiController> _Logger;

    public ProductsApiController(IProductData EmployeeData, ILogger<ProductsApiController> Logger)
    {
        _ProductData = EmployeeData;
        _Logger = Logger;
    }

    [HttpGet("sections")]
    public IActionResult GetSections() => Ok(_ProductData.GetSections().ToDTO());

    [HttpGet("sections/{Id:int}")]
    public IActionResult GetSectionById(int Id) => _ProductData.GetSectionById(Id) is {} section
        ? Ok(section.ToDTO())
        : NotFound(new {Id});

    [HttpGet("brands")]
    public IActionResult GetBrands() => Ok(_ProductData.GetBrands().ToDTO());

    [HttpGet("brands/{Id:int}")]
    public IActionResult GetBrandById(int Id) => _ProductData.GetBrandById(Id) is { } brand
        ? Ok(brand.ToDTO())
        : NotFound(new { Id });

    [HttpPost]
    public IActionResult GetProducts([FromBody] ProductFilter filter)
    {
        var products = _ProductData.GetProducts(filter);

        if(products.Any())
            return Ok(products.ToDTO());

        return NoContent();
    }

    [HttpGet("{Id:int}")]
    public IActionResult GetProductBuId(int Id) => _ProductData.GetProductById(Id) is { } product
        ? Ok(product.ToDTO())
        : NoContent();
}
