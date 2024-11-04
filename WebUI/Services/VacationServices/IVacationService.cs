using Application.DTOs;
using Application.Interfaces.Pagination;
using MediatR;

namespace WebUI.Services.VacationServices
{
    public interface IVacationService
    {
        Task<PaginationResult<VacationDto>> GetVacations(PaginationParameter queryModel);
        Task<PaginationResult<VacationDto>> GetVacations(PaginationParameter paginationParameter, Guid workerId);
        Task<List<VacationDto>> GetVacationsForMonth(int month, int year);
        Task<VacationCreateResultDto> VacationCreate(VacationDto request);
        Task<Unit> VacationDelete(Guid id);
        Task<Unit> VacationEdit(VacationDto request);
        Task<VacationDto> GetVacationById(Guid id);
    }
}
