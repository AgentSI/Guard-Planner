using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Roles.Commands
{
    public class RoleDeleteCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
    }

    public class RoleDeleteCommandHandler(IAppDbContext appDbContext) : IRequestHandler<RoleDeleteCommand, Unit>
    {
        private readonly IAppDbContext _appDbContext = appDbContext;

        public async Task<Unit> Handle(RoleDeleteCommand request, CancellationToken cancellationToken)
        {
            var roleToDelete = await _appDbContext.UserRoles.Where(e => e.Id == request.Id).FirstOrDefaultAsync();

            _appDbContext.UserRoles.Remove(roleToDelete!);
            await _appDbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
