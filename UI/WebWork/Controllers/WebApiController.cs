using Microsoft.AspNetCore.Mvc;
using WebWork.Intefaces.TestApi;

namespace WebWork.Controllers;

public class WebApiController:Controller
{
    private readonly IValuesService _ValuesServices;

    public WebApiController(IValuesService ValuesServices)
    {
        _ValuesServices = ValuesServices;
    }

    public IActionResult Index()
    {
        var values = _ValuesServices.GetValues();
        return View(values);  
    }
}
