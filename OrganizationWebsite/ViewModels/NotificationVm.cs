using System.ComponentModel.DataAnnotations;

namespace OrganizationWebsite.ViewModels
{
    public class NotificationVm
    {
        [Required(ErrorMessage ="Enter Notification")]
        public string? Notification {  get; set; }
        [Required(ErrorMessage ="Select Date")]
        public DateTime? Date { get; set; }
    }
}
