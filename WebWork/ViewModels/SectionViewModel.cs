
namespace WebWork.ViewModels;

public class SectionViewModel
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public List<SectionViewModel> ChildSections { get; set; } = new();
}
