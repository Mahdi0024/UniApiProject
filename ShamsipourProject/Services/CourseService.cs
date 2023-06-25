using ApiProject.Data;
using Microsoft.EntityFrameworkCore;
using UniApiProject.Models.Responses;

namespace ApiProject.Services;

public class CourseService
{
    private ApiDbContext _db;

    public CourseService(ApiDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<CourseItemResponse>> GetCourses(string searchText, int page)
    {
        var courses = _db.Courses.AsQueryable();
        if (!String.IsNullOrEmpty(searchText))
        {
            courses = courses.Where(c => c.Title.Contains(searchText));
        }
        if (page > 0)
        {
            courses = courses.Skip(page * 15);
        }
        var mappedCourses = await courses.Select(c =>
                    new CourseItemResponse(
                         new TeacherInfo(
                              c.TeacherId,
                             _db.Users.Where(u => u.UserId == c.TeacherId)
                                      .Select(t => t.FirstName + " " + t.LastName)
                                      .First()),
                         new CourseInfo(
                               c.CourseId,
                               c.Title,
                               c.ShortDescription,
                               c.Category.Name,
                               c.Price,
                               c.Discount,
                               c.DateAdded,
                               c.DateUpdated,
                               c.ViewCount,
                               c.Image.FileId)))
                                         .Take(15)
                                         .ToListAsync();
        return mappedCourses;
    }
}