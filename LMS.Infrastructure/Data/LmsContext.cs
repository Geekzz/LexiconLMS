using Domain.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infrastructure.Data;

public class LmsContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
{
    public LmsContext(DbContextOptions<LmsContext> options) : base(options)
    {
    }
    public DbSet<Activity> Activities { get; set; }
    public DbSet<ActivityType> ActivityTypes { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Module> Modules { get; set; }

    public DbSet<ActivityDocument> ActivityDocuments { get; set; }
    public DbSet<CourseDocument> CourseDocuments { get; set; }
    public DbSet<ModuleDocument> ModuleDocuments { get; set; }
}
