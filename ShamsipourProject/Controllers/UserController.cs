using ApiProject.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniApiProject.Exceptions;
using UniApiProject.Models.Requests;
using UniApiProject.Helpers;
using UniApiProject.Services;

namespace ApiProject.Controllers;

[Route("Api/User")]
public class UserController:ControllerBase
{
    private readonly ApiDbContext _db;
    private readonly UserService _userService;

    public UserController(ApiDbContext db,UserService userService)
    {
        _db = db;
        _userService = userService;
    }

    [Authorize(Roles = "Student,Teacher")]
    [HttpPut("updateDetails")]
    public async Task<IActionResult> UpdateDetails(UpdateUserDetailsRequest details)
    {
        var userId = HttpContext.User.GetUserId();

        var user = await _userService.GetUser(userId);
        
        user.FirstName = details.FirstName;
        user.LastName = details.LastName;
        user.Bio = details.Bio;

        await _userService.UpdateUser(user);
        return Ok();
    }



}