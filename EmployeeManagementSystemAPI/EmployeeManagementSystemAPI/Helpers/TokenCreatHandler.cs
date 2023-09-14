using EmployeeManagementSystemAPI.Context;
using EmployeeManagementSystemAPI.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace EmployeeManagementSystemAPI.Helpers
{
    public class TokenCreatHandler
    {
        private static IConfiguration _configuration;

        public static void Initialize(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public static ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            
            var key = Encoding.ASCII.GetBytes(_configuration["JwtSettings:Key"]);
            var tokenValidationParametrs = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidAudience = _configuration["JwtSettings:Audience"],
                ValidIssuer = _configuration["JwtSettings:Issuer"],
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateLifetime = false
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParametrs, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("This is invalid token");
            }
            return principal;
        }

        public static string CreateJWTToken(User user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["JwtSettings:Key"]);
            var identity = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            });
            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);
            int expirationMinutes = int.Parse(_configuration["JwtSettings:AccessTokenExpirationMinutes"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = identity,
                Expires = DateTime.Now.AddMinutes(expirationMinutes),
                SigningCredentials = credentials
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            return jwtTokenHandler.WriteToken(token);
        }

        public static string CreateRefreshJWTToken(AppDbContext _authContext)
        {
            var tokenBytes = RandomNumberGenerator.GetBytes(64);
            var refreshToken = Convert.ToBase64String(tokenBytes);

            var tokenInUser = _authContext.Users
                .Any(user => user.RefreshToken == refreshToken);
            if (tokenInUser)
            {
                return CreateRefreshJWTToken(_authContext);
            }
            return refreshToken;
        }
    }
}
