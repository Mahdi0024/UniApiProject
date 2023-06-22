using System.ComponentModel.DataAnnotations;

namespace ApiProject.Models;

public class Category
{
    [Key]
    public Guid CategoryId { get; set; }

    public string Name { get; set; } = null!;
}