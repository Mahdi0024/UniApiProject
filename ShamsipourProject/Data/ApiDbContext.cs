using ApiProject.Models;
using Microsoft.EntityFrameworkCore;
using File = ApiProject.Models.File;

namespace ApiProject.Data;

public class ApiDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<CourseProgress> CourseProgresses { get; set; }
    public DbSet<Banner> Banners { get; set; }
    public DbSet<File> Files { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Category> Categories { get; set; }

    public ApiDbContext(DbContextOptions options) : base(options)
    {
    }
}