using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OrganizationWebsite.Models;

namespace OrganizationWebsite.Data
{
    public class ApplicationDbContext:IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        {
        
        }
        public DbSet<Course> CourseTable { get; set; }
        public DbSet<Notification> NotificationTable { get; set; }
    }
}
