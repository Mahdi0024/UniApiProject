namespace UniApiProject.Models.Requests;

public class AddEpisodeRequest
{
    public Guid CourseId { get; set; }
    public string Description { get; set; } = null!;
    public TimeSpan Duration { get; set; }
    public bool Free { get; set; }
    public IFormFile File { get; set; } = null!;
    public int Index { get; set; }

}
