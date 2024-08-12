using System.ComponentModel.DataAnnotations;

namespace OrganizationWebsite.ViewModels
{
    public class UpdateVm
    {
        public string? Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public string? Gender { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string? Email { get; set; }

        public string? Phone_number { get; set; }

        public string? Country { get; set; }

        public string? State { get; set; }

        public string? City { get; set; }

        public string? Hobbie { get; set; }

        public string? Avatar { get; set; }

        public string? Picked_Courses { get; set; }
        public string? Status { get; set; }

    }
}
