using ApiProject.Data;
using ApiProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniApiProject.Exceptions;
using UniApiProject.Models.Requests;
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
                               c.Image.FileId,
                               c.Episodes.Select(e => new EpisodeInfo(e.EpisodeId, e.Index, e.Description, e.Free)))))
                                         .Take(15)
                                         .ToListAsync();
        return mappedCourses;
    }
    public async Task<CourseItemResponse> GetCourse(Guid id)
    {
        var course = await _db.Courses.Select(c =>
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
                               c.Image.FileId,
                               c.Episodes.Select(e => new EpisodeInfo(e.EpisodeId, e.Index, e.Description, e.Free)))))
            .FirstOrDefaultAsync();
        if(course is null)
        {
            throw new NotFoundException("The requested course does not exist.");
        }
        return course;
    }

    public async Task<IEnumerable<Category>> GetCategories()
    {
        return await _db.Categories.ToListAsync();
    }

    public async Task<Course> CreateCourse(Guid TeacherId,CreateCourseRequest data)
    {
        var newCourse = new Course()
        {
            TeacherId = TeacherId,
            Category = await _db.Categories.FirstAsync(c => c.CategoryId == data.CategoryId),
            Title = data.Title,
            DateAdded = DateTime.Now,
            DateUpdated = DateTime.Now,
            Description = data.Description,
            Discount = data.Discount,
            Price = data.Price,
            ShortDescription = data.ShortDescription,
            ViewCount = 0
        };
        _db.Courses.Add(newCourse);
        await _db.SaveChangesAsync();
        return newCourse;
    }


    public async Task SetCourseImage(SetCourseImageRequest request)
    {
        var courseId = request.CourseId;
        var course = await _db.Courses.FindAsync(courseId);
        if (course is null)
        {
            throw new NotFoundException("The requested course does not exist");
        }

        var image = request.file;

        var fileName = image.FileName;
        var tempFilename = $"/temp/{fileName}";
        using var stream = new FileStream(tempFilename, FileMode.CreateNew);
        await image.CopyToAsync(stream);

        var newFile = new Models.File()
        {
            FileName = fileName,
            Size = image.Length,
        };
        _db.Files.Add(newFile);
        await _db.SaveChangesAsync();

        System.IO.File.Move(tempFilename,$"/files/{newFile.FileId}");
        
        course.Image = newFile;
        await _db.SaveChangesAsync();
    }
}