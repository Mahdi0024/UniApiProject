namespace UniApiProject.Models.Requests;

public record SetCourseImageRequest(Guid CourseId,IFormFile file);