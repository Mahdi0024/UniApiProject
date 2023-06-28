namespace UniApiProject.Models.Requests;

public record CreateCourseRequest(string Title,
    string Description,
    string ShortDescription,
    long Price,
    int Discount,
    Guid CategoryId);