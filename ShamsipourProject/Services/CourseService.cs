using UniApiProject.Data;
using UniApiProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniApiProject.Exeptions;
using UniApiProject.Models.Requests;
using UniApiProject.Models.Responses;
using UniApiProject.Services;

namespace UniApiProject.Services;

public class CourseService
{
    private ApiDbContext _db;
    private readonly FileService _fileService;
    public CourseService(ApiDbContext db, FileService fileService)
    {
        _db = db;
        _fileService = fileService;
    }

    public async Task<IEnumerable<CourseItemResponse>> GetCourses(string? searchText, Guid? categoryId, int page)
    {
        var courses = _db.Cources.AsQueryable();
        if (!String.IsNullOrEmpty(searchText))
        {
            courses = courses.Where(c => c.Title.Contains(searchText));
        }
        if (page > 0)
        {
            courses = courses.Skip(page * 15);
        }
        if (categoryId != null)
        {
            courses = courses.Where(c => c.Category.CategoryId == categoryId);
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
                               _db.CourseProgresses.Count(p => p.CourseId == c.CourseId),
                               c.Image.FileId,
                               c.Lectures.Select(e => new EpisodeInfo(e.LectureId, e.Index, e.Description, e.Free)))))
                                         .Take(15)
                                         .ToListAsync();
        return mappedCourses;
    }
    public async Task<CourseItemResponse> GetCourse(Guid id)
    {
        var course = await _db.Cources.Select(c =>
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
                               _db.CourseProgresses.Count(p => p.CourseId == c.CourseId), //Enroled count
                               c.Image.FileId,
                               c.Lectures.Select(e => new EpisodeInfo(e.LectureId, e.Index, e.Description, e.Free)))))
            .FirstOrDefaultAsync();
        if (course is null)
        {
            throw new NotFoundException("The requested course does not exist.");
        }

        return course;
    }
    public async Task<IEnumerable<Category>> GetCategories()
    {
        return await _db.Categories.ToListAsync();
    }
    public async Task<Course> CreateCourse(Guid TeacherId, CreateCourseRequest data)
    {
        var lectures = data.Lectures.Select(l =>
            new Lecture()
            {
                Description = l.Description,
                Duration = l.Duration,
                Index = l.Index,
                Free = l.Free
            }
        ).ToList();

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
            ViewCount = 0,
            Lectures = lectures
        };
        _db.Cources.Add(newCourse);
        await _db.SaveChangesAsync();
        return newCourse;
    }
    public async Task SetCourseImage(SetCourseImageRequest request)
    {
        var course = await _db.Cources.FindAsync(request.CourseId);
        if (course is null)
        {
            throw new NotFoundException("The requested course does not exist");
        }

        var file = await _fileService.UploadFile(request.file);

        course.Image = file;
        await _db.SaveChangesAsync();
    }
    public async Task<Lecture?> UploadLecture(UploadLectureRequest request)
    {
        var lecture = await _db.Lectures.Include(l => l.File).Where(l => l.LectureId == request.LectureId).FirstOrDefaultAsync();
        if (lecture is null)
        {
            throw new NotFoundException("Requested lecture does not exist.");
        }

        var file = await _fileService.UploadFile(request.File);

        if (lecture.File is not null)
        {
            System.IO.File.Delete(lecture.File.FileName);
            _db.Files.Remove(lecture.File);
        }

        lecture.File = file;
        await _db.SaveChangesAsync();
        return lecture;

    }

    public async Task<IEnumerable<CommentResponse>> GetComments(Guid CourseId)
    {
        var comments = await _db.Comments.Where(c => c.WrittenTo.CourseId == CourseId)
            .Select(c => new CommentResponse
            {
                UserId = c.WrittenBy.UserId,
                Text = c.Text,
                Submited = c.Submited
            }).ToListAsync();
        return comments;
    }

    internal async Task<CommentResponse> AddComment(Guid userId, SendCommentRequest request)
    {
        var newComment = new Comment()
        {
            Submited = DateTime.Now,
            Text = request.Text,
            WrittenBy = _db.Users.Find(userId)!,
            WrittenTo = _db.Cources.Find(request.CourseId)!
        };
        _db.Comments.Add(newComment);
        await _db.SaveChangesAsync();

        return new CommentResponse()
        {
            Submited = newComment.Submited,
            UserId = userId,
            Text = request.Text,
        };
    }
}