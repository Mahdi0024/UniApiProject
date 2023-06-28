using ApiProject.Models;
using ApiProject.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniApiProject.Helpers;
using UniApiProject.Models.Requests;
using UniApiProject.Models.Responses;

namespace ApiProject.Controllers;

[Route("Api/Course")]
public class CourseController : ControllerBase
{
    private readonly CourseService _courseService;

    public CourseController(CourseService courseService)
    {
        _courseService = courseService;
    }

    [HttpGet("/search")]
    public async Task<IEnumerable<CourseItemResponse>> GetCourses(string search, int page = 0)
    {
        return await _courseService.GetCourses(search, page);
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


}