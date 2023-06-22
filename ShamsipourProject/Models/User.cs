using System.ComponentModel.DataAnnotations;

namespace ApiProject.Models;

public class User
{
    [Key]
    public Guid UserId { get; set; }

    public string Username { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? PhoneNumber { get; set; } = null!;
    public string? VerificationCode { get; set; } = null!;
    public byte[] PasswordHash { get; set; } = Array.Empty<byte>();
    public byte[] Salt { get; set; } = Array.Empty<byte>();
    public DateTime DateRegistered { get; set; }
    public UserRole Role { get; set; }
    public string? Bio { get; set; }
    public File? ProfilePicture { get; set; }
}