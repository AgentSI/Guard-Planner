using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Workers.Commands
{
    public class WorkerDeleteCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
    }

    public class WorkerDeleteCommandHandler : IRequestHandler<WorkerDeleteCommand, Unit>
    {
        private readonly IAppDbContext _appDbContext;

        public WorkerDeleteCommandHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<Unit> Handle(WorkerDeleteCommand request, CancellationToken cancellationToken)
        {
            var toDelete = await _appDbContext.Workers.Where(e => e.Id == request.Id).FirstOrDefaultAsync();

            _appDbContext.Workers.Remove(toDelete);
            await _appDbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
