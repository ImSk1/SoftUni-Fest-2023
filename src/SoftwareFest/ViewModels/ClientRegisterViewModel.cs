namespace SoftwareFest.ViewModels
{
    public class ClinetRegisterViewModel
    {
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;

        public UserViewModel User { get; set; } = default!;
    }
}
