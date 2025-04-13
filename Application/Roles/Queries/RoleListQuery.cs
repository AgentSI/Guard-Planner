using Application.DTOs;
using Application.Interfaces;
using Application.Interfaces.Pagination;
using MediatR;

namespace Application.Roles.Queries
{
    public class RoleListQuery(PaginationParameter paginationParameter) : IRequest<PaginationResult<UserRoleDto>>
    {
        public PaginationParameter PaginationParameter { get; set; } = paginationParameter;
    }

    public class RoleListQueryHandler(IPaginationService paginationService, IAppDbContext appDbContext) : IRequestHandler<RoleListQuery, PaginationResult<UserRoleDto>>
    {
        private readonly IPaginationService _paginationService = paginationService;
        private readonly IAppDbContext _appDbContext = appDbContext;

        public async Task<PaginationResult<UserRoleDto>> Handle(RoleListQuery request, CancellationToken cancellationToken)
        {
            var query = request.PaginationParameter;
            var roles = _appDbContext.UserRoles.AsQueryable();

            return await _paginationService.PaginateAsync(roles, query, RoleMapping.UserRoleProjection);
        }
    }
}
