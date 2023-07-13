namespace UniApiProject.Models.Responses;

public record CourseItemResponse(TeacherInfo Teacher,
                                 CourseInfo Course);
public record TeacherInfo(Guid Id,
                          string FullName);
public record CourseInfo(Guid CourseId,
                         string Title,
                         string ShortDescription,
                         string Category,
                         long Price,
                         int Discount,
                         DateTime Added,
                         DateTime Updated,
                         int Views,
                         int Enrolled,
                         Guid ImageFileId,
                         IEnumerable<EpisodeInfo> Episodes);

public record EpisodeInfo(Guid Id,int Index,string Description,bool Free);