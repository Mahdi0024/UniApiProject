using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using ApiProject.Data;
using ApiProject.Dtos;
using FileIO = System.IO.File;

namespace ApiProject.Controllers;

[Route("Api")]
public class FileController : ControllerBase
{
    private readonly ApiDbContext _db;
    const string filesDirectory = "Files";
    public FileController(ApiDbContext db)
    {
        _db = db;
        if (!Directory.Exists(filesDirectory))
        {
            Directory.CreateDirectory(filesDirectory);
        }
    }
    [HttpGet("Banners")]
    public async Task<IActionResult> GetBanners()
    {
        var banners = await _db.Banners.OrderBy(b => b.Index)
            .Select(b => new BannerDto 
            {
                BannerId = b.BannerID,
                FileId = b.File.FileId, Index = b.Index 
            })
            .ToListAsync();
        return Ok(banners);

    }
    [HttpGet("File")]
    public async Task<IActionResult> GetFile(Guid fileId)
    {
        var file = await _db.Files.FindAsync(fileId);
        if (file == null)
        {
            return NotFound();
        }
        var fileNameOnDisk = Path.Combine(filesDirectory, file.FileId.ToString(), file.Extension);

        var provider = new FileExtensionContentTypeProvider();
        string? contentType;
        if (!provider.TryGetContentType(fileNameOnDisk, out contentType))
        {
            contentType = "application/octet-stream";
        }
        using var fileStream = FileIO.OpenRead(fileNameOnDisk);

        return base.File(fileStream, contentType, file.FileName, true);
    }

}
