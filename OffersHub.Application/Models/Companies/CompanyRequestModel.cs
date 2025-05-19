namespace OffersHub.Application.Models.Companies
{
    public class CompanyRequestModel
    {
        public string UserName { get; set; } = string.Empty;
        public string Name {  get; set; } = string.Empty;
        public string Password {  get; set; } = string.Empty;
        public string? Image { get; set; }
        public byte[] ImageData { get; set; } = Array.Empty<byte>();

    }
}
