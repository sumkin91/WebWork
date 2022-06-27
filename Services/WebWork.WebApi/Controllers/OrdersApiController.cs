using Microsoft.AspNetCore.Mvc;
using WebWork.Domain.DTO;
using WebWork.Intefaces;
using WebWork.Intefaces.Services;

namespace WebWork.WebApi.Controllers;

[ApiController]
//[Route("api/orders")]
[Route(WebApiAddresses.V1.Orders)]
public class OrdersApiController : ControllerBase
{
    private readonly IOrderService _OrderService;
    private readonly ILogger<OrdersApiController> _Logger;

    public OrdersApiController(IOrderService OrderService, ILogger<OrdersApiController> Logger)
    {
        _OrderService = OrderService;
        _Logger = Logger;
    }

    [HttpGet("user/{UserName}")]
    public async Task<IActionResult> GetUserOrders(string UserName)
    {
        var orders = await _OrderService.GetUserOrdersAsync(UserName);
        if(orders.Any())
            return Ok(orders.ToDTO());

        return NoContent();
    }

    [HttpGet("{Id:int}")]
    public async Task<IActionResult> GetOrderById(int Id)
    {
        var order = await _OrderService.GetOrderByIdAsync(Id);
        if (order is null)
            return NotFound();

        return Ok(order.ToDTO());
    }

    [HttpPost("{UserName}")]
    public async Task<IActionResult> CreateOrder(string UserName, [FromBody] CreateOrderDTO Model)
    {
        var cart = Model.Items.ToCartView();
        var order_model = Model.Order;

        var order = await _OrderService.CreateOrderAsync(UserName, cart, order_model);

        return CreatedAtAction(nameof(GetOrderById), new {order.Id}, order.ToDTO());
    }
}
