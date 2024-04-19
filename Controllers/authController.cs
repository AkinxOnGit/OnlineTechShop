using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OnlineTechShop.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OnlineTechShop.Controllers;

    [AllowAnonymous]  
    [Route("api/token")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        public IConfiguration _configuration;
        private readonly TechOnlineShopContext _context;

        public TokenController(IConfiguration config, TechOnlineShopContext context)
        {
            _configuration = config;
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> login(UserInfo _userData)
        {
            if (_userData != null && _userData.Username != null && _userData.Password != null)
            {
                var user = await _context.UserTables.FirstOrDefaultAsync(u => u.Username == _userData.Username && u.Password == _userData.Password);

                if (user != null)
                {
                    var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
                        new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                        new Claim(ClaimTypes.Name, user.Username),
                        new Claim(ClaimTypes.Email, user.Email)
                    };
                    var key1 = _configuration["Jwt:Key"];
                    var key2 = Encoding.UTF8.GetBytes(key1);
                    var key = new SymmetricSecurityKey(key2);
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);
                    var token = new JwtSecurityToken(
                        _configuration["Jwt:Issuer"],
                        _configuration["Jwt:Audience"],
                        claims,
                        expires: DateTime.UtcNow.AddMinutes(60),
                        signingCredentials: signIn);

                    return Ok(new JwtSecurityTokenHandler().WriteToken(token));
                }
                else
                {
                    return BadRequest("Invalid credentials");
                }
            }
            else
            {
                return BadRequest();
            }
        }
    }

 

    public class UserInfo
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
    }

    public class RegisterUser
    {
        public string Username { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string Email { get; set; } = null!;
    }

