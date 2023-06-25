using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiProject.Models;

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
    public ICollection<Episode> Episodes { get; set; } = new List<Episode>();
    public DateTime DateAdded { get; set; }
    public DateTime DateUpdated { get; set; }
    public int ViewCount { get; set; }
}