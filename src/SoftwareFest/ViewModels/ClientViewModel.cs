namespace SoftwareFest.ViewModels
{
    using AutoMapper;
    using SoftwareFest.Models;

    public class ClientViewModel : UserViewModel
    {
        public int? Id { get; set; }
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string? UserId { get; set; }
        public void Mapping(Profile mapping)
        {
            mapping.CreateMap<Client, ClientViewModel>().ReverseMap();
        }
    }
}
