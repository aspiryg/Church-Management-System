using ChurchMS_Backend.Entities;
using ChurchMS_Backend.Models;

namespace ChurchMS_Backend.Services
{
    public interface IAuthService
    {
        Task<User> RegisterAsync (UserDto request);
        Task<string?> LoginAsync (UserDto request);
    }
}
