using Application.DTOs;
using Application.Interfaces.Pagination;
using MediatR;

namespace WebUI.Services.WorkerServices
{
    public interface IWorkerService
    {
        Task<PaginationResult<WorkerDto>> GetWorkers(PaginationParameter queryModel);
        Task<List<WorkerHoursDto>> GetWorkerHours(int month, int year);
        Task<Guid> WorkerCreate(WorkerDto request);
        Task<Unit> WorkerDelete(Guid id);
        Task<Unit> WorkerEdit(WorkerDto request);
        Task<WorkerDto> GetWorkerById(Guid id);
        Task<WorkerDto> GetWorkerByEmail(string email);

        Task<List<double>> GetPercentages();
        Task<List<PercentDto>> GetPercentList();
        Task<Guid> PercentCreate(double percent);
        Task<Unit> PercentDelete(Guid id);
    }
}
