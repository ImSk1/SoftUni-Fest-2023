namespace SoftwareFest.ViewModels
{
    public class ClientRegisterViewModel
    {
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;

        public UserViewModel User { get; set; } = default!;
    }
}
