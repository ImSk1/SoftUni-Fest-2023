namespace SoftwareFest.Services.Contracts
{
    using SoftwareFest.ViewModels;

    public interface IClientService
    {
        Task CreateClient(ClientViewModel business);
    }
}
