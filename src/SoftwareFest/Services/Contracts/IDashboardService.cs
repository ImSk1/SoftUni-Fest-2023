namespace SoftwareFest.Services.Contracts
{
    using SoftwareFest.ViewModels;

    public interface IDashboardService
    {
        Task<List<DashboardViewModel>> LoadFor(string userId);
    }
}
