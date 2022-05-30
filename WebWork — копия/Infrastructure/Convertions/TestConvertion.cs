using Microsoft.AspNetCore.Mvc.ApplicationModels;
using WebWork.Controllers;

namespace WebWork.Infrastructure.Convertions;

public class TestConvertion : IControllerModelConvention
{
    public void Apply(ControllerModel controller) //для каждого контроллера будет вызвана модель, с которой есть возможность работы
    {
        if(controller.ControllerName == "Home")
        {
            //модификация модели контроллера
            //controller.Actions.Add(new ActionModel(typeof(HomeController).GetMethod("TestMethod"), Array.Empty<object>()));
        }
    }
}
