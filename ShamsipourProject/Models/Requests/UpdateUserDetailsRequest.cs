namespace UniApiProject.Models.Requests;

public record UpdateUserDetailsRequest(string FirstName,
                                       string LastName,
                                       string Bio);

