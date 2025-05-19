using System.ComponentModel.DataAnnotations;

namespace OffersHub.Web.Models.ViewModels
{
    public class CompanyDashboardViewModel
    {
        public int CompanyId { get; set; }
        public string UserName { get; set; } = string.Empty;

        public string CompanyName { get; set; } = string.Empty;
        public string? Image {  get; set; } = string.Empty;
        //public IFormFile? ImageFile { get; set; }
        public bool IsActive { get; set; }
    }
}
