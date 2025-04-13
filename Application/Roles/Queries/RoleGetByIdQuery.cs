using Application.DTOs;
using Application.Interfaces;
using MediatR;

namespace Application.Roles.Queries
{
    public class RoleGetByIdQuery(Guid id) : IRequest<UserRoleDto>
    {
        public Guid Id { get; set; } = id;
    }

    public class RoleGetByIdQueryHandler(IAppDbContext appDbContext) : IRequestHandler<RoleGetByIdQuery, UserRoleDto>
    {
        private readonly IAppDbContext _appDbContext = appDbContext;

        public Task<UserRoleDto> Handle(RoleGetByIdQuery request, CancellationToken cancellationToken)
        {
            var role = _appDbContext.UserRoles.Where(p => p.Id == request.Id).Select(RoleMapping.UserRoleProjection).FirstOrDefault();
            if (role != null) return Task.FromResult(role);
            else return Task.FromResult(new UserRoleDto());
        }
    }
}
