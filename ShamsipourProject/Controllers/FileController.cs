using UniApiProject.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using UniApiProject.Models.Responses;
using UniApiProject.Services;
using FileIO = System.IO.File;

namespace UniApiProject.Controllers;

[Route("Api/File")]
public class FileController : ControllerBase
{
    private readonly ApiDbContext _db;
    private readonly FileService _fileService;

    public FileController(ApiDbContext db,FileService fileService)
    {
        _db = db;
        _fileService = fileService;
    }

    [HttpGet("banners")]
    public async Task<IActionResult> GetBanners()
    {
        var banners = await _db.Banners.OrderBy(b => b.Index)
            .Select(b => new BannerResponse(b.BannerID,b.File.FileId,b.Index))
            .ToListAsync();
        return Ok(banners);
    }

    [HttpGet("getFile")]
    public async Task<IActionResult> GetFile(Guid fileId)
    {
        var file = await _fileService.GetFileInfo(fileId);
        if(file is null)
        {
            return NotFound();
        }

        var provider = new FileExtensionContentTypeProvider();
        string? contentType;
        if (!provider.TryGetContentType(file.FullName, out contentType))
        {
            contentType = "application/octet-stream";
        }
        using var fileStream = file.OpenRead();

        return base.File(fileStream, contentType, file.Name, true);
    }


}