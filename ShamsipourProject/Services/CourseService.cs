using Microsoft.EntityFrameworkCore;
using ApiProject.Data;
using ApiProject.Dtos;
using ApiProject.Models;

namespace ApiProject.Services;

public record CourseService(ApiDbContext dbContext)
{
    public async Task<IEnumerable<ShortCourseDto>> GetCourses(string searchText, int page)
    {
        throw new NotImplementedException();
        //var courses = dbContext.Courses.AsQueryable();
        //if (!String.IsNullOrEmpty(searchText))
        //{
        //    courses = courses.Where(c => c.Title.Contains(searchText));
        //}
        //if (page > 0)
        //{
        //    courses = courses.Skip(page * 15);
        //}
        //var mappedCourses = await courses.Take(15)
        //    .Select(c =>
        //        new ShortCourseDto
        //        {
        //            Title = c.Title,
        //            Category = c.Category.Name,
        //            DateAdded = c.DateAdded,
        //            DateUpdated = c.DateUpdated,
        //            Id = c.CourseId,
        //            Discount = c.Discount,
        //            Price = c.Price,
        //            ShortDescription = c.ShortDescription,
        //            Teacher = c.Teacher.FirstName + " " + c.Teacher.LastName,
        //            ViewCount = c.ViewCount,
        //            Image = c.Image
        //        }).ToListAsync();
        //foreach (var course in mappedCourses)
        //{
        //    var courseImageBytes = File.ReadAllBytes(course.Image);
        //    course.Image = Convert.ToBase64String(courseImageBytes);
        //}
        //return mappedCourses;
    }
}
