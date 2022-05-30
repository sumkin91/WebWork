using Microsoft.AspNetCore.Mvc;
using WebWork.Services.Interfaces;
using WebWork.ViewModels;

namespace WebWork.Components;

//[ViewComponent (Name ="sections")]
public class SectionsViewComponent: ViewComponent
{
    private readonly IProductData _ProductData;

    public SectionsViewComponent(IProductData ProductData)
    {
        _ProductData = ProductData;
    }

    public IViewComponentResult Invoke()
    {
        var sections = _ProductData.GetSections();

        var parent_sections = sections.Where(s => s.ParentId is null).OrderBy(s =>s.Order);

        var parent_sections_views = parent_sections
            .Select(s => new SectionViewModel
            {
                Id = s.Id,
                Name = s.Name,
            })
            .ToArray();

        foreach (var parent_section in parent_sections_views)
        {
            var childs = sections.Where(s => s.ParentId == parent_section.Id);
            foreach (var child in childs.OrderBy(s => s.Order))
            {
                parent_section.ChildSections.Add(new()
                {
                    Id = child.Id,
                    Name = child.Name,
                });
            }
        }

        return View(parent_sections_views);
    }

    //public async Task<IViewComponentResult> InvokeAsync() => View();
}
