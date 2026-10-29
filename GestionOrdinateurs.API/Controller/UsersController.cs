using GestionOrdinateurs.Shared;
using Microsoft.AspNetCore.Mvc;
using GestionOrdinateurs.API.Service;

namespace GestionOrdinateurs.API.Controller;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<ActionResult<List<User>>> GetUsers()
    {
        return Ok(await _userService.GetUsers());
    }

    [HttpGet("{login}")]
    public async Task<ActionResult<User>> GetUserParLogin(string login)
    {
        var user = await _userService.GetUserParLogin(login);

        if (user == null)
        {
            return NotFound(new { Message = "Cet utilisateur n'existe pas." });
        }

        return Ok(user);
    }
}