namespace UniApiProject.Models;

public class Comment
{
    public Guid CommentId { get; set; }
    public User WrittenBy { get; set; } = null!;
    public string Text { get; set; } = null!;
    public Course WrittenTo { get; set; } = null!;
    public DateTime Submited { get; set; }
}
