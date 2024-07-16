using Microsoft.AspNetCore.Mvc;
using API_Movies.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Text;
using System;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;

namespace API_Movies.Controllers.Authentication
{
    [Route("[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly MoviesDbContext _context;
        private readonly IConfiguration _config;

        public LoginController(IConfiguration config, MoviesDbContext context)
        {
            _context = context;
            _config = config;
        }

        // Với case password ko bị mã hóa...
        //private User AuthenticateUser(User user)
        //{
        //    return _context.Users.FirstOrDefault(x => x.Email == user.Email && x.Password == user.Password);
        //}

        //với case password bị mã hóa bằng Laravel Breeze => Sử dụng BCrypt
        private User AuthenticateUser(UserDto loginUser)
        {
            var user = _context.Users.FirstOrDefault(x => x.Email == loginUser.Email);

            if (user != null && BCrypt.Net.BCrypt.Verify(loginUser.Password, user.Password))
            {
                return user;
            }

            return null;
        }

        private string GenerateToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login(UserDto user)
        {
            var authenticatedUser = AuthenticateUser(user);
            if (authenticatedUser != null)
            {
                var token = GenerateToken(authenticatedUser);
                return Ok(new { token });
            }

            return Unauthorized();
        }
    }
}
