using WebWork.Domain.ViewModels;

namespace WebWork.Intefaces.Services;

public interface ICartService
{
    void Add(int Id);

    void Remove(int Id);

    void Decrement(int Id);

    void Clear();
    CartViewModel GetViewModel();
}
