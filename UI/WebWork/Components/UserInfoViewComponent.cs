using Microsoft.AspNetCore.Mvc;

namespace WebWork.Components;

public class UserInfoViewComponent: ViewComponent
{
    public IViewComponentResult Invoke() => User.Identity!.IsAuthenticated
        ?View("UserInfo")
        :View();
}
