using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebWork.Domain.Entities.Identity;

namespace WebWork.Areas.Admin.Controllers;

//[Area("Admin")]
[Authorize(Roles = Role.Administrators)]
public class HomeController: Controller
{
    public IActionResult Index() => View();
}
