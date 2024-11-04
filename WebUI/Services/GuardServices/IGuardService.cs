using Application.DTOs;
using Application.Interfaces.Pagination;
using MediatR;

namespace WebUI.Services.GuardServices
{
    public interface IGuardService
    {
        Task<List<GuardDto>> GetGuards(int month, int year);
        Task<PaginationResult<GuardDto>> GetGuards(PaginationParameter queryModel);
        Task<PaginationResult<GuardDto>> GetGuards(PaginationParameter queryModel, Guid workerId);
        Task<Guid> GuardCreate(GuardDto request);
        Task<Unit> GuardDelete(Guid id);
        Task<Unit> GuardEdit(GuardDto request);
        Task<GuardDto> GetGuardById(Guid id);

        Task<List<WorkerDto>> GetWorkers();

        Task<List<GuardDto>> GenerateGuards(int month, int year);
        Task<Unit> SaveGuards(List<GuardDto> guards);
        Task<Unit> SendGuards(List<GuardDto> guards);
        Task<Unit> DeleteGuards(DateTime? date);
    }
}
