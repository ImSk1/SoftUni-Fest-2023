namespace SoftwareFest.ViewModels
{
    public class DetailsProductViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = default!;

        public string Description { get; set; } = default!;

        public long Price { get; set; }

        public string Category { get; set; } = default!;

        public string ImageUrl { get; set; } = default!;
    }
}
