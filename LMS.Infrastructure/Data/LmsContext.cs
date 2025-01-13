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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Course>().HasData(
            new Course
            {
                CourseId = 1,
                Name = "Fullstack.NET 2025",
                Description = "Lorem ipsum odor amet.",
                StartDate = new DateTime(2025, 01, 01),
                EndDate = new DateTime(2025, 01, 29),
                CourseDocuments = null,
                Modules = new List<Module>()
            });
        modelBuilder.Entity<Module>().HasData(
            new Module
            {
                ModuleId = 1,
                Name = "C#-basics",
                Description = "C# module",
                StartDate = new DateTime(2025, 01, 01),
                EndDate = new DateTime(2025, 01, 11),
                CourseId = 1,
                ModuleDocuments = null,
                Activities = new List<Activity>()
            });
        modelBuilder.Entity<ActivityType>().HasData(
            new ActivityType
            {
                ActivityTypeId = 1,
                Type = "Föreläsning"
            }
            );
        
        
        modelBuilder.Entity<Activity>().HasData(
            new Activity
            {
                ActivityId = 1,
                Name = "Föreläsning - C#",
                Description = "Asp.Net",
                StartDate = new DateTime(2025, 01, 09, 11, 0, 0),
                EndDate = new DateTime(2025, 01, 09, 15, 0, 0),
                ActivityTypeId = 1,
                ModuleId = 1,
                ActivityDocument = null
            },
            new Activity
            {
                ActivityId = 2,
                Name = "Föreläsning - Java",
                Description = "Spring Boot",
                StartDate = new DateTime(2025, 01, 10, 11, 0, 0),
                EndDate = new DateTime(2025, 01, 10, 15, 0, 0),
                ActivityTypeId = 1,
                ModuleId = 1,
                ActivityDocument = null
            }

        );


    }
}
