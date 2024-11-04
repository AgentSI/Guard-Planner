using Application.DTOs;
using Application.Interfaces.Pagination;
using MediatR;

namespace WebUI.Services.OperationServices
{
    public interface IOperationService
    {
        Task<PaginationResult<OperationDto>> GetOperations(PaginationParameter queryModel);
        Task<PaginationResult<OperationDto>> GetOperations(PaginationParameter queryModel, Guid guardId);
        Task<Guid> OperationCreate(OperationDto request);
        Task<Unit> OperationDelete(Guid id);
        Task<Unit> OperationEdit(OperationDto request);
        Task<OperationDto> GetOperationById(Guid id);
    }
}
