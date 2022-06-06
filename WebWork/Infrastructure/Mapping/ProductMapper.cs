using WebWork.Domain.Entities;
using WebWork.ViewModels;
using System.Diagnostics.CodeAnalysis;

namespace WebWork.Infrastructure.Mapping;

public static class ProductMapper
{
    [return: NotNullIfNotNull("product")]
    public static ProductViewModel? ToView(this Product product) => product is null
        ? null
        : new ProductViewModel
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            ImageUrl = product.ImageUrl,
            Brand = product?.Brand?.Name,
            Section = product?.Section?.Name,
        };

    public static IEnumerable<ProductViewModel?> ToView(this IEnumerable<Product> products) =>
        products.Select(p => p.ToView());
}
