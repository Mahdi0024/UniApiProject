using System.ComponentModel.DataAnnotations;

namespace UniApiProject.Models;

public class Category
{
    [Key]
    public Guid CategoryId { get; set; }

    public string Name { get; set; } = null!;
}