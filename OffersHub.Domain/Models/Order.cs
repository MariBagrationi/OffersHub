
namespace OffersHub.Domain.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty; 
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public bool IsDeleted { get; set; } = false;
        public decimal TotalPrice { get; set; }
        public List<OrderItem> Items { get; set; } = new();
    }

}
