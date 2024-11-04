using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Operations.Commands
{
    public class OperationDeleteCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
    }

    public class OperationDeleteCommandHandler : IRequestHandler<OperationDeleteCommand, Unit>
    {
        private readonly IAppDbContext _appDbContext;

        public OperationDeleteCommandHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<Unit> Handle(OperationDeleteCommand request, CancellationToken cancellationToken)
        {
            var toDelete = await _appDbContext.Operations.Where(e => e.Id == request.Id).FirstOrDefaultAsync();

            _appDbContext.Operations.Remove(toDelete);
            await _appDbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
