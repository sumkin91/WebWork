﻿using System.Net.Http.Json;
using WebWork.Domain;
using WebWork.Domain.DTO;
using WebWork.Domain.Entities;
using WebWork.Intefaces.Services;
using WebWork.WebApi.Clients.Base;

namespace WebWork.WebApi.Clients;

public class ProductsClient: BaseClient, IProductData
{
    public ProductsClient(HttpClient Client): base(Client, "api/products"){ }

    public Brand? GetBrandById(int Id)
    {
        var result = Get<BrandDTO>($"{Address}/brands/{Id}");
        return result.FromDTO();
    }

    public IEnumerable<Brand> GetBrands()
    {
        var result = Get<IEnumerable<BrandDTO>>($"{Address}/brands");
        return result.FromDTO();
    }

    public Product? GetProductById(int Id)
    {
        var result = Get<ProductDTO>($"{Address}/{Id}");
        return result.FromDTO();
    }

    public IEnumerable<Product> GetProducts(ProductFilter? Filter = null)
    {
        var response = Post(Address, Filter ?? new());

        if(response.StatusCode == System.Net.HttpStatusCode.NoContent)
            return Enumerable.Empty<Product>();

        var result = response
            .EnsureSuccessStatusCode()
            .Content
            .ReadFromJsonAsync<IEnumerable<ProductDTO>>()
            .Result;

        return result.FromDTO();
    }

    public Section? GetSectionById(int Id)
    {
        var result = Get<SectionDTO>($"{Address}/sections/{Id}");
        return result.FromDTO();
    }

    public IEnumerable<Section> GetSections()
    {
        var result = Get<IEnumerable<SectionDTO>>($"{Address}/sections");
        return result.FromDTO();
    }
}
