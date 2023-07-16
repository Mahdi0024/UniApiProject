namespace UniApiProject.Models.Requests;

public record CreateCourseRequest(string Title,
    string Description,
    string ShortDescription,
    long Price,
    int Discount,
    Guid CategoryId,
    LectureInfo[] Lectures);
public record LectureInfo(string Description,TimeSpan Duration,bool Free, int Index);