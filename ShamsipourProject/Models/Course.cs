using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniApiProject.Models;

public class Course
{
    [Key]
    public Guid CourseId { get; set; }

    [ForeignKey("TeacherId")]
    public Guid TeacherId { get; set; }

    [ForeignKey("CategoryId")]
    public Category Category { get; set; } = null!;

    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string ShortDescription { get; set; } = null!;
    public File Image { get; set; } = null!;
    public long Price { get; set; }
    public int Discount { get; set; }
    public ICollection<Lecture> Lectures { get; set; } = new List<Lecture>();
    public DateTime DateAdded { get; set; }
    public DateTime DateUpdated { get; set; }
    public int ViewCount { get; set; }
}