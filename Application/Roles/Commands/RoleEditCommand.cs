using Application.DTOs;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Roles.Commands
{
    public class RoleEditCommand(UserRoleDto model) : IRequest<Unit>
    {
        public Guid Id { get; set; } = model.Id;
        public string? Role { get; set; } = model.RoleName;
    }

    public class RoleEditCommandHandler(IAppDbContext appDbContext) : IRequestHandler<RoleEditCommand, Unit>
    {
        private readonly IAppDbContext _appDbContext = appDbContext;

        public async Task<Unit> Handle(RoleEditCommand request, CancellationToken cancellationToken)
        {
            var roleToEdit = await _appDbContext.UserRoles
                .Where(p => p.Id == request.Id)
                .FirstOrDefaultAsync();

            if (roleToEdit != null)
            {
                roleToEdit.RoleName = request.Role;
            }

            await _appDbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
