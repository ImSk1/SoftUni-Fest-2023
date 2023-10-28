namespace SoftwareFest.Services.Contracts
{
    using System.Linq.Expressions;

    using SoftwareFest.Models.Enums;
    using SoftwareFest.Pagination.Contracts;
    using SoftwareFest.Pagination.Enums;
    using SoftwareFest.ViewModels;

    public interface IProductService
    {
        Task<IPage<ShowProductViewModel>> GetPagedProducts(string name, int pageIndex = 1, int pageSize = 50,
            Expression<Func<Models.Product, object>>? orderBy = null,
            SortDirection sortDirection = SortDirection.Ascending);
        Task<IPage<ShowProductViewModel>> GetPagedProductsByUserId(string userId, int pageIndex = 1, int pageSize = 50);

        Task AddProduct(ProductViewModel model, string userId);

        Task<ProductViewModel> GetById(int productId, string userId);

        Task<ProductViewModel> GetById(int productId);

        Task Update(ProductViewModel model);

        Task Delete(int id);

        Task<bool> IsOwner(string userId, int productId);

        Task<bool> HasEnoughQuantity(int productId);
    }
}
