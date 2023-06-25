using ApiProject.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using UniApiProject.Models.Responses;
using FileIO = System.IO.File;

namespace ApiProject.Controllers;

[Route("Api")]
public class FileController : ControllerBase
{
    private readonly ApiDbContext _db;
    private const string filesDirectory = "Files";

    public FileController(ApiDbContext db)
    {
        _db = db;
        if (!Directory.Exists(filesDirectory))
        {
            Directory.CreateDirectory(filesDirectory);
        }
    }

    [HttpGet("banners")]
    public async Task<IActionResult> GetBanners()
    {
        var banners = await _db.Banners.OrderBy(b => b.Index)
            .Select(b => new BannerResponse(b.BannerID,b.File.FileId,b.Index))
            .ToListAsync();
        return Ok(banners);
    }

    [HttpGet("file")]
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