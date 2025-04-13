using Application.DTOs;
using MediatR;

namespace WebUI.Services.HourServices
{
    public interface IHourService
    {
        Task<List<HourDto>> GetHourList();
        Task<Guid> HourCreate(HourDto hour);
        Task<Unit> HourDelete(Guid id);
    }
}
