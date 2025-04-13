using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Roles.Commands
{
    public class RoleCreateCommand(UserRoleDto model) : IRequest<Unit>
    {
        public Guid Id { get; set; } = model.Id;
        public string? UserRole { get; set; } = model.RoleName;
    }

    public class RoleCreateCommandHandler(IAppDbContext appDbContext) : IRequestHandler<RoleCreateCommand, Unit>
    {
        private readonly IAppDbContext _appDbContext = appDbContext;

        public async Task<Unit> Handle(RoleCreateCommand request, CancellationToken cancellationToken)
        {
            var existingRole = _appDbContext.UserRoles.FirstOrDefault(u => u.RoleName == request.UserRole);
            if (existingRole == null)
            {
                var role = new UserRole
                {
                    RoleName = request.UserRole
                };

                _appDbContext.UserRoles.Add(role);
                await _appDbContext.SaveChangesAsync(cancellationToken);
            }

            return Unit.Value;
        }
    }
}
