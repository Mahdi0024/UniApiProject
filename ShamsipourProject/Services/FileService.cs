using ApiProject.Data;
using ApiProject.Models;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using File = ApiProject.Models.File;

namespace UniApiProject.Services;

public class FileService
{
    private readonly string _filesDirectory;
    private readonly string _tempDirectory;
    private readonly ApiDbContext _db;
    public FileService(string filesDirectory, string tempDirectory, ApiDbContext db)
    {
        _db = db;
        _filesDirectory = filesDirectory;
        if (!Directory.Exists(filesDirectory))
        {
            Directory.CreateDirectory(filesDirectory);
        }
        _tempDirectory = tempDirectory;
        if (!Directory.Exists(tempDirectory))
        {
            Directory.CreateDirectory(tempDirectory);
        }
    }


    public async Task<FileInfo?> GetFileInfo(Guid fileId)
    {
        var file = await _db.Files.FindAsync(fileId);
        if (file is null)
        {
            return null;
        }
        var filePath = Path.Combine(_filesDirectory, file.FileId.ToString(), file.Extension);
        return new FileInfo(filePath);
    }
    public async Task<File> UploadFile(IFormFile formFile)
    {
        var file = new File()
        {
            FileId = Guid.NewGuid(),
            FileName = formFile.FileName,
            Size = formFile.Length,
        };

        var tempFileName = Path.Combine(_tempDirectory, file.FileId.ToString());

        try
        {
            using var fileStream = new FileStream(tempFileName, FileMode.CreateNew);
            await formFile.CopyToAsync(fileStream);
            System.IO.File.Move(tempFileName, Path.Combine(_filesDirectory, file.FileId.ToString(), file.Extension));

            _db.Files.Add(file);
            await _db.SaveChangesAsync();
            return file;
        }
        finally
        {
            if (System.IO.File.Exists(tempFileName))
            {
                System.IO.File.Delete(tempFileName);
            }
        }
    }


}
