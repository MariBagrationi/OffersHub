
namespace OffersHub.Domain.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; } = null!;

        public int OfferId { get; set; }
        public Offer Offer { get; set; } = null!;

        public int Quantity { get; set; }
        public decimal Price { get; set; } // Store price at the time of purchase
        public bool IsDeleted { get; set; } = false;
    }

}
