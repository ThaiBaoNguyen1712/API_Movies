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
using API_Movies.Helpers;
using API_Movies.Services;
using Microsoft.EntityFrameworkCore;
using API_Movies.Models.DTO;

namespace API_Movies.Controllers.Authentication
{
    [Route("/User")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly MoviesDbContext _context;
        private readonly IConfiguration _config;
        private readonly IEmailService emailService;
        public UserController(IConfiguration config, MoviesDbContext context, IEmailService emailService)
        {
            _context = context;
            _config = config;
            this.emailService = emailService;   
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
        private async Task SendMail(string toEmail, string subject, string body)
        {
            try
            {
                // Tạo mailrequest từ các tham số nhận được
                Mailrequest mailrequest = new Mailrequest
                {
                    ToEmail = toEmail,
                    Subject = subject,
                    Body = body
                };

                // Gọi service để gửi email và đợi kết quả
                await emailService.SendEmailAsync(mailrequest);
            }
            catch (Exception ex)
            {
                // Xử lý lỗi khi gửi email, có thể log lại lỗi hoặc xử lý phù hợp
                Console.WriteLine("Failed to send email: " + ex.Message);
                throw; // Re-throw để cho phép lỗi tiếp tục được xử lý ở mức cao hơn nếu cần
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public IActionResult Login(UserDto user)
        {
            try
            {
                var authenticatedUser = AuthenticateUser(user);
                if (authenticatedUser != null)
                {
                    if (authenticatedUser.Role.Contains("admin"))
                    {
                        var token = GenerateToken(authenticatedUser);
                        return Ok(new { token });
                    }
                    else
                    {
                        return Ok(new { message = "Login Successful" });
                    }
                }
                else
                {
                    return Unauthorized(new { message = "Invalid credentials" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred during the login process", error = ex.Message });
            }
        }

        //Register and generate VerifyCode 
        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> Register(User user)
        {
            try
            {
                var existingUser = await _context.Users.FirstOrDefaultAsync(x => x.Email == user.Email);
                if (existingUser != null)
                {
                    return BadRequest(new { message = "Email already in use." });
                }
                user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
                user.VerificationCode = Guid.NewGuid();
                user.IsVerified = 0;
                user.Role = "client";
                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                string body = "Here is your Verification Code to join with us: " + user.VerificationCode;
                string subject = "Verify Your Email Address";

                // Gọi hàm SendMail để gửi email xác nhận
                await SendMail(user.Email, subject, body);

                return Ok(new { message = "Please check your email for verification." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpPost("VerifyEmail")]
        public async Task<IActionResult> VerifyEmail([FromBody] VerifyPassword verifyPassword)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.VerificationCode == verifyPassword.VerificationCode);
            if(user ==null)
            {
                return NotFound(new { message = "Invalid credentials" });
            }
            try
            {
                user.IsVerified = 1;
                await _context.SaveChangesAsync();
                return Ok(new { message = "Verify successful" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //Function change verifyCode
        [AllowAnonymous]
        [HttpPost("ChangeVerifyCode")]
        public async Task<IActionResult> ChangeVerifyCode(UserDto userdto)
        {
            var user = AuthenticateUser(userdto);
            if (user == null)
            {
                return NotFound();
            }
            try
            {
                user.VerificationCode = Guid.NewGuid();
                user.IsVerified = 0;
                await _context.SaveChangesAsync();

                string body = "Here is your Verification Code to join with us: " + user.VerificationCode;
                string subject = "Verify Your Email Address";

                // Gọi hàm SendMail để gửi email xác nhận
                await SendMail(user.Email, subject, body);
                return Ok(new { message = "Please check your email for verification." });
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
         
        }
        [AllowAnonymous]
        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] string email)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
                if(user==null)
                {
                    return NotFound(new { message = "User Not Found" });
                }
                user.VerificationCode = Guid.NewGuid();
                user.IsVerified = 0;
                await _context.SaveChangesAsync();

                string body = $"Here is your Password Reset Code: {user.VerificationCode}";
                string subject = "Password Reset Request";
                await SendMail(user.Email, subject, body);

                return Ok(new { message = "Please check your email for the password reset code." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.VerificationCode == resetPasswordDto.VerificationCode);
                if (user == null || user.IsVerified != 0)
                {
                    return BadRequest(new { message = "Invalid or expired verification code." });
                }

                user.Password = BCrypt.Net.BCrypt.HashPassword(resetPasswordDto.NewPassword);
                user.VerificationCode = null;
                user.IsVerified = 1;
                await _context.SaveChangesAsync();

                return Ok(new { message = "Password reset successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        //Changepassword 
        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
        {
            var userDto = new UserDto { Email = changePasswordDto.Email, Password = changePasswordDto.Password };
            var user = AuthenticateUser(userDto);
            if (user == null)
            {
                return NotFound(new { message = "Invalid user credentials" });
            }
            try
            {
                user.Password = BCrypt.Net.BCrypt.HashPassword(changePasswordDto.NewPassword);
                await _context.SaveChangesAsync();
                return Ok(new { message = "Password changed successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        //Remove Account
        [HttpDelete("DeleteAccount/{id}")]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
                if(user==null)
                {
                    return NotFound(new { message = "User not found" });
                }
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                return Ok(new { message="Delete account successful" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}
