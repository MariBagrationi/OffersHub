namespace OffersHub.Application.Models.Clients
{
    public class ClientServiceModel
    {
        public int Id { get; set; } 
        public string Name { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public decimal Balance { get; set; }
        public string? ImageUrl { get; set; }
    }
}
