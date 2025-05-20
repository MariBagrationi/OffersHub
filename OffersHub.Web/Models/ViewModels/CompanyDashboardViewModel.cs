using System.ComponentModel.DataAnnotations;

namespace OffersHub.Web.Models.ViewModels
{
    public class CompanyDashboardViewModel
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Image {  get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
