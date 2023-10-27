using SoftwareFest.Models;
using SoftwareFest.ViewModels;

namespace SoftwareFest.Services.Contracts
{
    public interface IBusinessService
    {
        Task CreateBusiness(BusinessViewModel business);
        Task<BusinessViewModel> GetBusinessById(int id);
        Task UpdateBusiness(BusinessViewModel business);
        Task<bool> DeleteBusiness(int id);
    }
}
