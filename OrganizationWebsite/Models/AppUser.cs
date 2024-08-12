using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace OrganizationWebsite.Models
{
    public class AppUser:IdentityUser
    {
        public string? FirstName {  get; set; }
        public string? LastName { get; set; }

        public string? Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? Country { get; set; }
        public string? State { get; set; }
        public string? City { get; set; }
        public string? Hobbies { get; set; }
        public string? Avatar { get; set; }
        public string? Picked_Courses { get; set; }
        public string? Status { get; set; } = "Pending";
    }
}
