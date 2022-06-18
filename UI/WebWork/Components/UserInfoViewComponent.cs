using Microsoft.AspNetCore.Mvc;
using WebWork.Services.Interfaces;
using WebWork.ViewModels;

namespace WebWork.Components;

public class UserInfoViewComponent: ViewComponent
{
    public IViewComponentResult Invoke() => User.Identity!.IsAuthenticated
        ?View("UserInfo")
        :View();
}
