namespace UniApiProject.Models.Requests;

public record UploadLectureRequest(IFormFile File,Guid LectureId);

