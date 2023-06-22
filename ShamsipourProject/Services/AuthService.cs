using ApiProject.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ApiProject.Services;

public class AuthService
{
    private readonly IConfiguration _configuration;
    private readonly SigningCredentials _signingCredentials;
    private readonly JwtSecurityTokenHandler _tokenHandler;

    public AuthService(IConfiguration configuration)
    {
        _configuration = configuration;
        var secKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!));
        _signingCredentials = new SigningCredentials(secKey, SecurityAlgorithms.HmacSha512);
        _tokenHandler = new JwtSecurityTokenHandler();
    }

    public bool ValidatePassword(string password, byte[] hashedPassword, byte[] salt)
    {
        using var sha512 = new HMACSHA512(salt);
        var inputPasswordHash = sha512.ComputeHash(Encoding.UTF8.GetBytes(password));
        return inputPasswordHash.SequenceEqual(hashedPassword);
    }

    public (byte[] hash, byte[] salt) GenerateHashAndSalt(string password)
    {
        using var sha512 = new HMACSHA512();
        var hash = sha512.ComputeHash(Encoding.UTF8.GetBytes(password));
        return (hash, sha512.Key);
    }

    public string GenerateJsonWebToken(User user)
    {
        var claims = new Claim[]
        {
            new("id",user.UserId.ToString()),
            new("username",user.Username),
            new(ClaimTypes.Role,user.Role.ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(20),
            signingCredentials: _signingCredentials
            );

        var encodedToken = _tokenHandler.WriteToken(token);
        return encodedToken;
    }
}