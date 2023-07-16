using UniApiProject.Models;
using UniApiProject.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniApiProject.Helpers;
using UniApiProject.Models.Requests;
using UniApiProject.Models.Responses;
using UniApiProject.Services;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace UniApiProject.Controllers;

[Route("Api/Course")]
public class CourseController : ControllerBase
{
    private readonly CourseService _courseService;
    private readonly FileService _fileService;

    public CourseController(CourseService courseService, FileService fileService)
    {
        _courseService = courseService;
        _fileService = fileService;
    }

    [HttpGet("/search")]
    public async Task<IEnumerable<CourseItemResponse>> GetCourses(GetCoursesRequest request)
    {
        return await _courseService.GetCourses(request.SearchText, request.CategoryId, request.Page);
    }

    [HttpGet("/get")]
    public async Task<CourseItemResponse> GetCourse(Guid id)
    {
        return await _courseService.GetCourse(id);

    }

    [HttpPost("/create")]
    [Authorize(Roles = "Teacher")]
    public async Task<Course> CreateCourse(CreateCourseRequest request)
    {
        var teacherId = HttpContext.User.GetUserId();
        return await _courseService.CreateCourse(teacherId, request);

    }

    [HttpPost("/setImage")]
    [Authorize(Roles = "Teacher")]
    public async Task<IActionResult> SetCourseImage(SetCourseImageRequest request)
    {
        await _courseService.SetCourseImage(request);
        return Ok();
    }

    [HttpGet("GetComments")]
    public async Task<IEnumerable<CommentResponse>> Comments(Guid CourseId)
    {
        return await _courseService.GetComments(CourseId);
    }
    [HttpPost("SendComment")]
    [Authorize(Roles ="Student,Teacher")]
    public async Task<CommentResponse> SendComment(SendCommentRequest request)
    {
        var userId = HttpContext.User.GetUserId();
        var comment = await _courseService.AddComment(userId, request);
        return comment;
    }


}