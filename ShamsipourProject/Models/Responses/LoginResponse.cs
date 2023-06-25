namespace UniApiProject.Models.Responses;


public record LoginResponse(UserInfo User,
                            string Token);
public record UserInfo(Guid Id,
                       string FirstName,
                       string LastName,
                       string Username);
