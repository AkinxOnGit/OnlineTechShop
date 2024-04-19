using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineTechShop.Models;

namespace OnlineTechShop.Controllers;

[Produces("application/json")]  
[Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]  
[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{

    private readonly TechOnlineShopContext _context;

    private readonly ILogger<UserController> _logger;

    public UserController(ILogger<UserController> logger, TechOnlineShopContext context)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet("getUserByUsername")]
    public async Task<ActionResult<UserTable>> GetUserByUsername()
    {
        var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    
        var user = await _context.UserTables.Where(u => u.UserId == Int16.Parse(id)).FirstOrDefaultAsync();
        if (user == null)
        {
            return NotFound();
        }
        return Ok(user);
    }
}
