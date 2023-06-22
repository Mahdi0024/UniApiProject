using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiProject.Data;
using ApiProject.Dtos;
using ApiProject.Exceptions;
using ApiProject.Services;
using Azure.Identity;
using ApiProject.Models;

namespace ApiProject.Controllers;

public class AuthController : ControllerBase
{
    private readonly ApiDbContext _db;
    private readonly AuthService _authService;
    public AuthController(ApiDbContext db, AuthService authService)
    {
        _db = db;
        _authService = authService;

    }
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginModel credentials)
    {
        var user = await _db.Users.Where(u => u.Username.ToLower() == credentials.Username.ToLower())
            .FirstOrDefaultAsync();
        if (user == null || !_authService.ValidatePassword(credentials.Password, user.PasswordHash, user.Salt))
        {
            throw new AuthException("Invalid username or password");
        }

        return Ok(new { Token = _authService.GenerateJsonWebToken(user) });

    }
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterModel data)
    {
        var dupCheck = await _db.Users
            .Where(u => u.Username.ToLower() == data.Username.ToLower() || u.Email == data.Email.ToLower())
            .Select(u => new
            {
                DupUsername = u.Username.ToLower() == data.Username.ToLower(),
                DupEmail = u.Email == data.Email.ToLower()
            })
            .FirstOrDefaultAsync();
        if (dupCheck is not null)
        {
            if (dupCheck.DupEmail)
            {
                throw new RegisterException("Email is already in use");
            }
            if (dupCheck.DupUsername)
            {
                throw new RegisterException("Username is already in use");
            }
        }

        var (passwordHash, passwordSalt) = _authService.GenerateHashAndSalt(data.Password);
        var newUser = new User()
        {
            Username = data.Username,
            PasswordHash = passwordHash,
            Salt = passwordSalt,
            Email = data.Email.ToLower(),
            FirstName = data.FirstName,
            LastName = data.LastName,
            DateRegistered = DateTime.Now,
        };
        _db.Users.Add(newUser);
        await _db.SaveChangesAsync();

        return Ok(new { Token = _authService.GenerateJsonWebToken(newUser) });
    }

}
