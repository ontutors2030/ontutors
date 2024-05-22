using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OPTFS.Models;

namespace OPTFS.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {        
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<OPTFS.Models.Specialty> Specialty { get; set; } = default!;
        public DbSet<OPTFS.Models.Country> Country { get; set; } = default!;
        public DbSet<OPTFS.Models.City> City { get; set; } = default!;
        public DbSet<OPTFS.Models.CourseDetail> CourseDetail { get; set; } = default!;        
        public DbSet<OPTFS.Models.Subject> Subject { get; set; } = default!;
        public DbSet<OPTFS.Models.Course> Course { get; set; } = default!;
        public DbSet<OPTFS.Models.StudentCourse> StudentCourses { get; set; } = default!;        
        public DbSet<OPTFS.Models.StudentRequest> StudentRequest { get; set; } = default!;
        public DbSet<OPTFS.Models.UserFile> UserFile { get; set; } = default!;
        public DbSet<OPTFS.Models.Notification> Notification { get; set; } = default!;
        public DbSet<OPTFS.Models.PaymentModels.Payment> Payments { get; set; } = default!;
        public DbSet<OPTFS.Models.PaymentModels.PSource> PSources { get; set; } = default!;
        public DbSet<OPTFS.Models.Chat> Chat { get; set; } = default!;
        public DbSet<OPTFS.Models.ChatMessage> ChatMessages { get; set; } = default!;        
    }
}
