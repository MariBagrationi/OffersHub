namespace OffersHub.Application.Models.Offers
{
    public class OfferRequestModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? Image { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public DateTime OfferDueDate { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string Company_UserName { get; set; } = string.Empty;
    }
}
