namespace UniApiProject.Models.Responses;

public class CommentResponse
{
    public Guid UserId { get; set; }
    public string Text { get; set; } = null!;
    public DateTime Submited { get; set; }

}

