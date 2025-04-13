using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Users.Commands
{
    public class UserDeleteCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
    }

    public class UserDeleteCommandHandler(IAppDbContext appDbContext) : IRequestHandler<UserDeleteCommand, Unit>
    {
        private readonly IAppDbContext _appDbContext = appDbContext;

        public async Task<Unit> Handle(UserDeleteCommand request, CancellationToken cancellationToken)
        {
            var userToDelete = await _appDbContext.Users.Where(e => e.Id == request.Id).FirstOrDefaultAsync();

            _appDbContext.Users.Remove(userToDelete!);
            await _appDbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
