using SoftwareFest.ViewModels;

namespace SoftwareFest.Services.Contracts
{
    public interface ICheckoutService
    {
        Task<string> CheckOut(ProductViewModel product);
    }
}
