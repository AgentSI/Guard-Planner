using Application.DTOs;
using Application.Interfaces.Pagination;
using MediatR;

namespace WebUI.Services.RoleServices
{
    public interface IRoleService
    {
        Task<Unit> RoleCreate(UserRoleDto request);
        Task<Unit> RoleDelete(Guid id);
        Task<Unit> RoleEdit(UserRoleDto request);
        Task<UserRoleDto> GetRoleById(Guid id);
        Task<PaginationResult<UserRoleDto>> GetRoles(PaginationParameter queryModel);
    }
}
