using PsecKit.Core.Application.DTOs;
using System.Threading.Tasks;

namespace PsecKit.Core.Application.Contracts.Services
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto);
        Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
    }
}
