using WebWork.Domain;
using WebWork.DAL.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using WebWork.Domain.Entities.Identity;
using WebWork.Domain.Entities.Orders;
using WebWork.Domain.ViewModels;
using WebWork.Intefaces.Services;

namespace WebWork.Services.InSQL;

public class SqlOrderService: IOrderService
{
    private readonly WebWorkDB _db;
    private readonly UserManager<User> _UserManager;
    private readonly ILogger<SqlOrderService> _Logger;

    public SqlOrderService(WebWorkDB db, UserManager<User> UserManager, ILogger<SqlOrderService> Logger)
    {
        _db = db;
        _UserManager = UserManager;
        _Logger = Logger;
    }

    public async Task<IEnumerable<Order>> GetUserOrdersAsync(string UserName, CancellationToken Cancel = default)
    {
        var orders = await _db.Orders
           .Include(order => order.User)
           .Include(order => order.Items)
           .Where(order => order.User!.UserName == UserName)
           .ToArrayAsync(Cancel)
           .ConfigureAwait(false);

        return orders;
    }

    public async Task<Order?> GetOrderByIdAsync(int Id, CancellationToken Cancel = default)
    {
        var order = await _db.Orders
           .Include(order => order.User)
           .Include(order => order.Items)
           .FirstOrDefaultAsync(order => order.Id == Id, Cancel)
           .ConfigureAwait(false);

        return order;
    }

    public async Task<Order> CreateOrderAsync(string UserName, CartViewModel Cart, OrderViewModel OrderModel, CancellationToken Cancel = default)
    {
        var user = await _UserManager.FindByNameAsync(UserName).ConfigureAwait(false);

        if (user is null)
            throw new InvalidOperationException($"Пользователь с именем {UserName} в системе не найден");

        await using var transaction = await _db.Database.BeginTransactionAsync(Cancel);

        var order = new Order
        {
            User = user,
            Address = OrderModel.Address,
            Phone = OrderModel.Phone,
            Description = OrderModel.Description,
        };

        var products_ids = Cart.Items.Select(item => item.Product.Id).ToArray();

        var cart_products = await _db.Products
           .Where(p => products_ids.Contains(p.Id))
           .ToArrayAsync(Cancel);

        order.Items = Cart.Items.Join(
            cart_products,
            cart_item => cart_item.Product.Id,
            cart_product => cart_product.Id,
            (cart_item, cart_product) => new OrderItems
            {
                Order = order,
                Product = cart_product,
                Price = cart_product.Price,     //vожно модифицировать цены - скидку, бонусы
                Quantity = cart_item.Quantity,
            }).ToArray();

        await _db.Orders.AddAsync(order, Cancel);
        await _db.SaveChangesAsync(Cancel);

        await transaction.CommitAsync(Cancel);

        return order;
    }
}
