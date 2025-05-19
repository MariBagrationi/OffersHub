namespace OffersHub.Application.Models.Orders
{
    public class OrderServiceModel
    {
        public int Id { get; set; } 
        public string userName {  get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; } = string.Empty;
        public decimal TotalPrice { get; set; }
    }
}
