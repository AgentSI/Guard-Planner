using Application.DTOs;

namespace WebUI.Services.TokenServices
{
    public interface IMeService
    {
        Task<ClaimsDto> Me(string jwt);
    }
}
