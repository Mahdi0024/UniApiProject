using System.ComponentModel.DataAnnotations;

namespace ApiProject.Models;

public class File
{
    [Key]
    public Guid FileId { get; set; }
    public string FileName { get; set; } = null!;

    public string Extension { get => Path.GetExtension(FileName); }
    public long Size { get; set; }
}
