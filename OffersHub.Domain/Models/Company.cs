using OffersHub.Domain.Models;

namespace OffersHub.Domain.Models
{
    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Image { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public User User { get; set; } = null!;
        public List<Offer> Offers { get; set; } = new List<Offer>();
    }
}
