using MInimarketDaniela_Backend.DTOs;
using MInimarketDaniela_Backend.Models.DataModels;

namespace MInimarketDaniela_Backend.Services
{
    public interface IUserService
    {
        Task<User> RegisterAsync(RegisterUserDto userDto);
        Task<string> LoginAsync(LoginDto loginDto);
    }
}
