namespace OffersHub.Application.Models.Companies
{
    public class CompanyResponseModel
    {
        public int Id { get; set; }
        //public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Image { get; set; }
        public bool IsActive { get; set; }
        //public List<Product> Products { get; set; } = new();
    }
}
