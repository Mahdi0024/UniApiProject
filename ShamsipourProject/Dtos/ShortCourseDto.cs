namespace ApiProject.Dtos;

public class ShortCourseDto
{
    public int Id { get; set; }
    public string Teacher { get; set; }
    public string Title { get; set; }
    public string Image { get; set; }
    public string ShortDescription { get; set; }
    public string Category { get; set; }
    public long Price { get; set; }
    public int Discount { get; set; }
    public DateTime DateAdded { get; set; }
    public DateTime DateUpdated { get; set; }
    public int ViewCount { get; set; }
}
