using EmployeeManagementSystemAPI.Models.Dto;
using EmployeeManagementSystemAPI.Models;

namespace EmployeeManagementSystemAPI.Repository.Interfaces
{
    public interface IUserService
    {
        Task<TokenDto> AuthenticateAsync(LoginModelDto user);
        Task<string> RegisterAsync(User user);
        Task<TokenDto> RefreshTokenAsync(TokenDto tokenDto);
    }
}
