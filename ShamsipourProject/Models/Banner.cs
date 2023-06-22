using System.ComponentModel.DataAnnotations;

namespace ApiProject.Models;

public class Banner
{
    [Key]
    public Guid BannerID { get; set; }
    public File File { get; set; } = null!;
    public int Index { get; set; }
}
