using System.Text.Json;
using WebWork.Domain.Entities;
using WebWork.Domain.ViewModels;
using WebWork.Intefaces.Services;
using Microsoft.AspNetCore.Http;
using WebWork.Services.Mapping;

namespace WebWork.Services.Services.InCookies;

public class InCookiesCartService : ICartService
{
    private readonly IProductData _ProductData;
    private readonly IHttpContextAccessor _HttpContextAccessor;
    private readonly string _CartName;

    //де-/се-риализация корзины в json
    private Cart Cart
    {
        get
        {
            var context = _HttpContextAccessor.HttpContext;
            var cookies = context.Response.Cookies;

            var cart_cookie = context.Request.Cookies[_CartName];
            if (cart_cookie is null)
            {
                var cart = new Cart();
                cookies.Append(_CartName, JsonSerializer.Serialize(cart));
                return cart;
            }
            ReplaceCart(cookies, cart_cookie);
            return JsonSerializer.Deserialize<Cart>(cart_cookie)!;//десериализация корзины из куки
        }
        set => ReplaceCart(_HttpContextAccessor.HttpContext!.Response.Cookies, JsonSerializer.Serialize(value)); //сериализация корзины в кук
    }

    private void ReplaceCart(IResponseCookies cookies, string cart)
    {
        cookies.Delete(_CartName);
        cookies.Append(_CartName, cart);
    }

    public void Add(int Id)
    {
        var cart = Cart;
        cart.Add(Id);
        Cart = cart;
    }

    public void Remove(int Id)
    {
        var cart = Cart;
        cart.Remove(Id);
        Cart = cart; ;
    }

    public void Decrement(int Id)
    {
        var cart = Cart;
        cart.Decrement(Id);
        Cart = cart;
    }

    public void Clear()
    {
        var cart = Cart;
        cart.Clear();
        Cart = cart;
    }

    public CartViewModel GetViewModel()
    {
        var cart = Cart;
        var products = _ProductData.GetProducts(new()
        {
            Ids = cart.Items.Select(item => item.ProductId).ToArray(),
        });

        var products_views = products.ToView().ToDictionary(p => p.Id);

        return new()
        {
            Items = cart.Items
            .Where(item => products_views.ContainsKey(item.ProductId))//отсеиваем по Ids
            .Select(item => (products_views[item.ProductId], item.Quantity))!,//формируем кортежи ItemCart
        };
    }

    public InCookiesCartService(IProductData ProductData, IHttpContextAccessor HttpContextAccessor)
    {
        _ProductData = ProductData;
        _HttpContextAccessor = HttpContextAccessor;

        var user = _HttpContextAccessor.HttpContext!.User;
        var user_name = user.Identity!.IsAuthenticated ? $"-{user.Identity.Name}" : null;

        _CartName = $"WebWork.GB.Cart{user_name}";
    }
}
