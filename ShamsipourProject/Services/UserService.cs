using UniApiProject.Data;
using UniApiProject.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;
using UniApiProject.Exeptions;
using UniApiProject.Models.Requests;
using File = UniApiProject.Models.File;

namespace UniApiProject.Services;

public class UserService
{
    private readonly ApiDbContext _db;
    private readonly FileService _fileService;

    public UserService(ApiDbContext db, FileService fileService)
    {
        _db = db;
        _fileService = fileService;
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
    public async Task<File> SetUserImage(SetUserImageRequest request)
    {
        var user = await _db.Users.Include(u => u.ProfilePicture).FirstOrDefaultAsync(u => u.UserId == request.UserId);
        if (user is null)
        {
            throw new NotFoundException("The requested user does not exists");
        }
        if (user.ProfilePicture is not null)
        {
            _fileService.DeleteFile(user.ProfilePicture);
            _db.Files.Remove(user.ProfilePicture);
        }

        var newImage = await _fileService.UploadFile(request.File);

        user.ProfilePicture = newImage;

        await _db.SaveChangesAsync();

        return newImage;

    }

}
