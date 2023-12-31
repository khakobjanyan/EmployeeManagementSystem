﻿using EmployeeManagementSystemAPI.Context;
using EmployeeManagementSystemAPI.Helpers;
using EmployeeManagementSystemAPI.Models;
using EmployeeManagementSystemAPI.Models.Dto;
using EmployeeManagementSystemAPI.Repository.Interfaces;
using EmployeeManagementSystemAPI.Utility.EmailService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Authentication;
using System.Security.Cryptography;

namespace EmployeeManagementSystemAPI.Controllers
{
    /// <summary>
    /// Controller for managing user authentication and registration.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _authContext;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserController"/> class.
        /// </summary>
        /// <param name="appDbContext">The application database context.</param>
        /// <param name="configuration">The configuration for JWT tokens.</param>
        /// <param name="emailService">The email service for sending emails.</param>
        public UserController(AppDbContext appDbContext, IConfiguration configuration, IEmailService emailService, IUserService userService, ILogger<UserController> logger)
        {
            _authContext = appDbContext;
            _configuration = configuration;
            _emailService = emailService;
            _userService = userService;
            _logger = logger;   

        }

        /// <summary>
        /// Authenticates a user by verifying their credentials.
        /// </summary>
        /// <param name="userObj">The user's login credentials.</param>
        /// <returns>A response containing an access token and refresh token or an error message.</returns>
        /// 
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] LoginModelDto userObj)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _userService.AuthenticateAsync(userObj);
                    return Ok(result);
                }
                catch (AuthenticationException ex)
                {
                    return Unauthorized(new { Message = ex.Message });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message, ex);
                    return StatusCode(500, new { Message = "Internal server error" });
                }

            }
            else
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return BadRequest(new
                {
                    Message = "Validation failed",
                    Errors = errors
                });
            }

        }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="userObj">The user's registration information.</param>
        /// <returns>A response indicating success, a token, or validation errors.</returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User userObj)
        {
            if (userObj == null)
            {
                return BadRequest();
            }
            if (ModelState.IsValid)
            {
                var token = await _userService.RegisterAsync(userObj);

                return Ok(new
                {
                    Token = token,
                    Message = "User Registered!"
                });
            }
            else
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return BadRequest(new
                {
                    Message = "Validation failed",
                    Errors = errors
                });
            }

        }

        /// <summary>
        /// Refreshes an access token using a valid refresh token.
        /// </summary>
        /// <param name="tokenDto">The token data containing the access and refresh tokens.</param>
        /// <returns>A response containing a new access token or an error message.</returns>

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(TokenDto tokenDto)
        {
            try
            {
                var result = await _userService.RefreshTokenAsync(tokenDto);
                return Ok(result);
            }
            catch (SecurityTokenException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return StatusCode(500, new { Message = "Internal server error" });
            }

        }

        /// <summary>
        /// Sends a reset password email to the user.
        /// </summary>
        /// <param name="email">The user's email address.</param>
        /// <returns>A response indicating success or an error message.</returns>

        [HttpPost("send-reset-email/{email}")]
        public async Task<IActionResult> SendEmail(string email)
        {
            var user = await _authContext.Users.FirstOrDefaultAsync(u=> u.Email == email);
            if(user is null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "User not found"
                });
            }

            var tokenBytes = RandomNumberGenerator.GetBytes(64);
            var emailToken = Convert.ToBase64String(tokenBytes);
            user.ResetPasswordToken = emailToken;
            user.ResetPasswordExpired = DateTime.Now.AddMinutes(15);
            string from = _configuration["EmailSettings:From"];
            var emailModel = new EmailModel(email, "Reset Password", EmailBody.EmailStringBody(email, emailToken));
            _emailService.SendEmail(emailModel);
            _authContext.Entry(user).State = EntityState.Modified;
            await _authContext.SaveChangesAsync();
            return Ok(new
            {
                StatusCode = 200,
                Message = "Email Sent"
            });
        }

        /// <summary>
        /// Resets a user's password using a valid email token.
        /// </summary>
        /// <param name="resetPasswordDto">The data containing email, email token, and new password.</param>
        /// <returns>A response indicating success or an error message.</returns>

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            var newToken = resetPasswordDto.EmailToken.Replace(" ", "+");
            var user = await _authContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == resetPasswordDto.Email);
            if (user is null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "User doesn`t exist"
                });
            }

            var tokenCode = user.ResetPasswordToken;
            DateTime emailTokenExpiry = user.ResetPasswordExpired;
            if(tokenCode != resetPasswordDto.EmailToken || emailTokenExpiry < DateTime.Now)
            {
                return BadRequest(new
                {
                    StatusCode = 400,
                    Message = "Invalid reset link"
                });
            }

            user.Password = PasswordHasher.HashPassword(resetPasswordDto.NewPassword);
            _authContext.Entry(user).State |= EntityState.Modified;
            await _authContext.SaveChangesAsync();
            return Ok(new
            {
                StatusCode = 200,
                Message = "Password reset successfuly"
            });
        }

    }
}
