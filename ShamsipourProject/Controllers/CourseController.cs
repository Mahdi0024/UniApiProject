using Microsoft.AspNetCore.Mvc;
using ApiProject.Dtos;
using ApiProject.Services;
using System.Dynamic;

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
    public async Task<IActionResult> GetCourses(string searchString, int page = 0)
    {
        var courses = await _courseService.GetCourses(searchString, page);
        return Ok(courses);
    }
    [HttpGet("/")]
    public async Task<IActionResult> GetCourse(Guid id)
    {
        throw new NotImplementedException();
    }
}
