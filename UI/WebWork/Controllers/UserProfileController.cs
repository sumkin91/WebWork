using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebWork.Intefaces.Services;
using WebWork.Domain.ViewModels;

namespace WebWork.Controllers;

[Authorize]
public class UserProfileController : Controller
{
    public IActionResult Index() => View();

    public async Task<IActionResult> Orders([FromServices] IOrderService Orders)
    {
        var orders = await Orders.GetUserOrdersAsync(User.Identity!.Name!);

        return View(orders.Select(order => new UserOrderViewModel
        {
            Id = order.Id,
            Address = order.Address,
            Phone = order.Phone,
            Description = order.Description!,
            Date = order.Date,
            TotalPrice = order.TotalPrice,
        }));
    }
}