namespace SoftwareFest.Services.Contracts
{
    using SoftwareFest.Pagination.Contracts;
    using SoftwareFest.ViewModels;

    public interface ITransactionService
    {
        Task Create(int productId, string userId, string stripeTransactionId = "");

        Task<IPage<TransactionViewModel>> GetPagedTransactions(string userId, int pageIndex = 1, int pageSize = 50);
    }
}
