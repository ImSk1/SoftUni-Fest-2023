namespace SoftwareFest.ViewModels
{
    using SoftwareFest.Infrastructure.Mapping;
    using SoftwareFest.Models;

    public class ShowProductViewModel : IMapFrom<Product>
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string ImageUrl { get; set; } = null!;

        public long Price { get; set; }

    }
}
