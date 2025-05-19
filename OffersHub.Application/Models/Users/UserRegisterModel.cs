namespace OffersHub.Application.Models.Users
{
    public class UserRegisterModel
    {
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string Token {  get; set; } = string.Empty;
    }
}
