namespace UniApiProject.Models.Requests;

public record GetCoursesRequest(string? SearchText,Guid? CategoryId,int Page);
