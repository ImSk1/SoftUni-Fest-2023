namespace SoftwareFest.Services.Contracts
{
    
    using SoftwareFest.ViewModels;

    public interface IProductService
    {
        Task<List<ShowProductViewModel>> GetProducts();

        Task AddProduct(AddProductViewModel model, string userId);

        Task<DetailsProductViewModel> GetById(int id);
    }
}
