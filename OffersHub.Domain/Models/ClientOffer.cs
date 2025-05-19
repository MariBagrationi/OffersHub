
namespace OffersHub.Domain.Models
{
    public class ClientOffer
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public Client Client { get; set; } = null!;

        public int OfferId { get; set; }
        public Offer Offer { get; set; } = null!;

        public bool IsDeleted { get; set; }
        public int Quantity { get; set; } 
        public DateTime DateAdded { get; set; } = DateTime.UtcNow;
    }
}
