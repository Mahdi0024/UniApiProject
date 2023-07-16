using UniApiProject.Models;
using Microsoft.EntityFrameworkCore;
using File = UniApiProject.Models.File;

namespace UniApiProject.Data;

public class ApiDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<CourseProgress> CourseProgresses { get; set; }
    public DbSet<Banner> Banners { get; set; }
    public DbSet<File> Files { get; set; }
    public DbSet<Course> Cources { get; set; }
    public DbSet<Lecture> Lectures { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Comment> Comments { get; set; }

    public ApiDbContext(DbContextOptions options) : base(options)
    {
    }
}