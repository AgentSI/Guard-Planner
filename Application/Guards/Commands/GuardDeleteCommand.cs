using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Guards.Commands
{
    public class GuardDeleteCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
    }

    public class GuardDeleteCommandHandler(IAppDbContext appDbContext) : IRequestHandler<GuardDeleteCommand, Unit>
    {
        private readonly IAppDbContext _appDbContext = appDbContext;

        public async Task<Unit> Handle(GuardDeleteCommand request, CancellationToken cancellationToken)
        {
            var toDelete = await _appDbContext.Guards.Where(e => e.Id == request.Id).FirstOrDefaultAsync(cancellationToken: cancellationToken);

            _appDbContext.Guards.Remove(toDelete!);
            await _appDbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
