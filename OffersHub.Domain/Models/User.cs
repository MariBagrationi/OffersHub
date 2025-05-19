namespace OffersHub.Domain.Models
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public Role Role { get; set; }
        public string? Token { get; set; }
        public DateTime TokenExpiration { get; set; }
        public bool IsDeleted { get; set; }
       
    }
}
