namespace SoftwareFest.Services.Contracts
{
    using System.Linq.Expressions;

    using SoftwareFest.Models;
    using SoftwareFest.Pagination.Contracts;
    using SoftwareFest.Pagination.Enums;
    using SoftwareFest.ViewModels;

    public interface IProductService
    {
        Task<IPage<ShowProductViewModel>> GetPagedProducts(int pageIndex = 1, int pageSize = 50, Expression<Func<Product, bool>>? predicate = null, Expression<Func<ShowProductViewModel, object>>? orderBy = null, SortDirection sortDirection = SortDirection.Ascending);

        Task AddProduct(AddProductViewModel model, string userId);

        Task<DetailsProductViewModel> GetById(int id);
    }
}
