
namespace OffersHub.Domain.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }  
        public List<Offer> Offers { get; set; } = new();
        public bool IsDeleted { get; set; }
    }
}
