using System.ComponentModel.DataAnnotations;

namespace OrganizationWebsite.ViewModels
{
    public class RegisterVm
    {
        [Required(ErrorMessage = "Name is required")]
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        public string? Gender { get; set; }

        [Required(ErrorMessage = "Date of birth is required")]
        [Display(Name = "Date Of Birth")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Enter email address")]
        [EmailAddress]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Enter phone number")]
        [Phone]
        public string? Phone_number { get; set; }

        [Required(ErrorMessage = "Country is required")]
        public string? Country { get; set; }

        [Required(ErrorMessage = "State is required")]
        public string? State { get; set; }

        [Required(ErrorMessage = "City is required")]
        public string? City { get; set; }

        public string? Hobbie { get; set; }

        [Required]
        [DataType(DataType.Upload)]
        public IFormFile? Avatar { get; set; }

        [Required]
        [DataType(DataType.Password)]

        public string? Password { get; set; }

        [Compare("Password", ErrorMessage = "Passwords don't match")]
        [Display(Name = "Confirm password")]
        [DataType(DataType.Password)]
        public string? ConfirmPassword { get; set; }

    }
}
