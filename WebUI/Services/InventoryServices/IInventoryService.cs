using Application.DTOs;
using Application.Interfaces.Pagination;
using MediatR;

namespace WebUI.Services.InventoryServices
{
    public interface IInventoryService
    {
        Task<PaginationResult<InventoryDto>> GetInventories(PaginationParameter queryModel, Guid guardId);
        Task<Guid> InventoryCreate(InventoryDto request);
        Task<Unit> InventoryDelete(Guid id);
        Task<Unit> InventoryEdit(InventoryDto request);
        Task<InventoryDto> GetInventoryById(Guid id);
    }
}
