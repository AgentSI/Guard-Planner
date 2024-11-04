using Application.DTOs;

namespace Application.Interfaces
{
    public interface IUserService
    {
        Task<AuthResultDto> RegisterAsync(RegisterDto registerDto);
        Task<AuthResultDto> LoginAsync(LoginDto loginDto);
    }
}
