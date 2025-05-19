
namespace OffersHub.Domain.Models
{
    public class Client
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public User User { get; set; } = null!;
        public decimal Balance { get; set; }
        public bool IsDeleted { get; set; }
        public List<ClientOffer> Cart { get; set; } = new();
        public List<Order> Orders { get; set; } = new();
    }
}
