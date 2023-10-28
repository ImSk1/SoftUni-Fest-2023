namespace SoftwareFest.Services.Contracts
{
    using SoftwareFest.Pagination;
    using SoftwareFest.ViewModels;

    public interface IRetailerService
    {
        Task<Page<RetailerViewModel>> GetPagedProducts(int pageIndex, int pageSize, string name);
        Task<Page<ShowProductViewModel>> GetPagedProductsByRetailerId(int retailerId, int pageIndex, int pageSize, string name);
    }
}
