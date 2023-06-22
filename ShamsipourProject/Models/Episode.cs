using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiProject.Models;

public class Episode
{
    [Key]
    public Guid EpisodeId { get; set; }

    [ForeignKey("CourseId")]
    public Guid CourseId { get; set; }

    public int Index { get; set; }
    public string Description { get; set; } = default(string)!;
    public TimeSpan Duration { get; set; }

    public File? File { get; set; }
    public bool Free { get; set; }
}