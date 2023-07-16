using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniApiProject.Models;

public class CourseProgress
{
    [Key]
    public Guid ProgressId { get; set; }

    [ForeignKey("StudentId")]
    public Guid StudentId { get; set; }

    [ForeignKey("CourseId")]
    public Guid CourseId { get; set; }

    [ForeignKey("LectureId")]
    public Guid LectureId { get; set; }

    public bool Completed { get; set; }
    public DateTime CompleteDate { get; set; }
}