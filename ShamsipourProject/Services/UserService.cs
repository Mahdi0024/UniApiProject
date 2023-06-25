using ApiProject.Data;
using ApiProject.Models;
using System.Data;

namespace UniApiProject.Services;

public class UserService
{
    private readonly ApiDbContext _db;

    public UserService(ApiDbContext db)
    {
        _db = db;   
    }

    public async Task<User> GetUser(Guid userId)
    {
        var user = await _db.Users.FindAsync(userId);
        if (user == null)
        {
            throw new DataException("user not found");
        }
        return user;
    }

    public async Task UpdateUser(User user)
    {
        _db.Users.Update(user);
        await _db.SaveChangesAsync();
    }

}
