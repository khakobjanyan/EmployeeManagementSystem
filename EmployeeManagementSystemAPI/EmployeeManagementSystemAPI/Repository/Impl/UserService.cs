using EmployeeManagementSystemAPI.Context;
using EmployeeManagementSystemAPI.Models.Dto;
using EmployeeManagementSystemAPI.Models;
using EmployeeManagementSystemAPI.Repository.Interfaces;
using EmployeeManagementSystemAPI.Utility.EmailService;
using EmployeeManagementSystemAPI.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Security.Authentication;
using Microsoft.IdentityModel.Tokens;

namespace EmployeeManagementSystemAPI.Repository.Impl
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _authContext;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;

        public UserService(AppDbContext authContext, IConfiguration configuration, IEmailService emailService)
        {
            _authContext = authContext;
            _configuration = configuration;
            _emailService = emailService;
        }

        public async Task<TokenDto> AuthenticateAsync(LoginModelDto userObj)
        {
            var user = await _authContext.Users.FirstOrDefaultAsync(u => u.Email == userObj.Email);

            if (user == null || !PasswordHasher.VerifyPassword(userObj.Password, user.Password))
            {
                throw new AuthenticationException("Incorrect email or password.");
            }

            user.Token = TokenCreatHandler.CreateJWTToken(user);
            user.RefreshToken = TokenCreatHandler.CreateRefreshJWTToken(_authContext);
            user.RefreshTokenExpTime = DateTime.Now.AddHours(5);
            await _authContext.SaveChangesAsync();

            return new TokenDto
            {
                AccessToken = user.Token,
                RefreshToken = user.RefreshToken
            };

        }

        public async Task<string> RegisterAsync(User userObj)
        {
                userObj.Password = PasswordHasher.HashPassword(userObj.Password);
                userObj.Role = userObj.Role;
                userObj.Token = TokenCreatHandler.CreateJWTToken(userObj);
                await _authContext.Users.AddAsync(userObj);
                await _authContext.SaveChangesAsync();

            return userObj.Token;
        }

        public async Task<TokenDto> RefreshTokenAsync(TokenDto tokenDto)
        {
            // Refresh token logic here
            if (tokenDto == null)
            {
                throw new SecurityTokenException("Invalid token data");
            }

            if (string.IsNullOrEmpty(tokenDto.AccessToken))
            {
                throw new SecurityTokenException("Invalid request");
            }

            var principal = TokenCreatHandler.GetPrincipalFromExpiredToken(tokenDto.AccessToken);
            var userName = principal.Identity.Name;
            var user = await _authContext.Users.FirstOrDefaultAsync(u => u.UserName == userName);

            if (user == null || user.RefreshToken != tokenDto.RefreshToken || user.RefreshTokenExpTime <= DateTime.Now)
            {
                throw new SecurityTokenException("Invalid token");
            }

            var newAccessToken = TokenCreatHandler.CreateJWTToken(user);
            var newRefreshToken = TokenCreatHandler.CreateRefreshJWTToken(_authContext);
            user.RefreshToken = newRefreshToken;
            await _authContext.SaveChangesAsync();

            return new TokenDto
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            };
        }


    }
}
