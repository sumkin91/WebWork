using WebWork.Domain;
using WebWork.ViewModels;
using WebWork.Domain.Entities.Orders;

namespace WebWork.Services.Interfaces;

public interface IOrderService
{
    Task<IEnumerable<Order>> GetUserOrdersAsync(string UserName, CancellationToken Cancel = default);

    Task<Order?> GetOrderByIdAsync(int Id, CancellationToken Cancel = default);

    Task<Order> CreateOrderAsync(string UserName, CartViewModel Cart, OrderViewModel OrderModel, CancellationToken Cancel = default);
}
