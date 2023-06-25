namespace UniApiProject.Models.Requests;

public record UpdatePasswordRequest(string CurrentPassword,
                                    string NewPassword);

