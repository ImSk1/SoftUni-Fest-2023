using SoftwareFest.Infrastructure.Mapping;
using SoftwareFest.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SoftwareFest.ViewModels
{
    public class RetailerViewModel : IMapFrom<Business>
    {
        public int Id { get; set; }

        public string BusinessName { get; set; }

        public string UserId { get; set; }

        public List<Product> Products { get; set; }
    }
}
