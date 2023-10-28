namespace SoftwareFest.Services.Contracts
{
    using System.Linq.Expressions;

    using SoftwareFest.Pagination.Contracts;
    using SoftwareFest.Pagination.Enums;
    using SoftwareFest.ViewModels;

    public interface ITransactionService
    {
        Task Create(int productId, string userId, string stripeTransactionId);

        Task<IPage<TransactionViewModel>> GetPagedProducts(int pageIndex = 1, int pageSize = 50);
    }
}
