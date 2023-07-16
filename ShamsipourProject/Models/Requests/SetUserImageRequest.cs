namespace UniApiProject.Models.Requests;

public record SetUserImageRequest(IFormFile File, Guid UserId);
