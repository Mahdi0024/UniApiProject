namespace UniApiProject.Models.Requests;

public record RegisterRequest(string FirstName,
                              string LastName,
                              string Username,
                              string Password,
                              string Email,
                              UserRole UserRole);
