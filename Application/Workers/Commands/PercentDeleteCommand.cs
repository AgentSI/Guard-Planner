using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Workers.Commands
{
    public class PercentDeleteCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
    }

    public class PercentDeleteCommandHandler : IRequestHandler<PercentDeleteCommand, Unit>
    {
        private readonly IAppDbContext _appDbContext;

        public PercentDeleteCommandHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<Unit> Handle(PercentDeleteCommand request, CancellationToken cancellationToken)
        {
            var toDelete = await _appDbContext.Percents.Where(e => e.Id == request.Id).FirstOrDefaultAsync();

            _appDbContext.Percents.Remove(toDelete);
            await _appDbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
