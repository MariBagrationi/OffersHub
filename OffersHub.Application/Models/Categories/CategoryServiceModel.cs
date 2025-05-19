namespace OffersHub.Application.Models.Categories
{
    public class CategoryServiceModel
    {
        public int Id { get; set; } 
        public string Name { get; set; } = string.Empty;
        public string? Image {  get; set; }
        public string? Description { get; set; }
    }
}
