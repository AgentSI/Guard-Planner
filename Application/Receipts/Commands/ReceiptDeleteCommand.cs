using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Receipts.Commands
{
    public class ReceiptDeleteCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
    }

    public class ReceiptDeleteCommandHandler : IRequestHandler<ReceiptDeleteCommand, Unit>
    {
        private readonly IAppDbContext _appDbContext;

        public ReceiptDeleteCommandHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<Unit> Handle(ReceiptDeleteCommand request, CancellationToken cancellationToken)
        {
            var toDelete = await _appDbContext.Receipts.Where(e => e.Id == request.Id).FirstOrDefaultAsync();

            _appDbContext.Receipts.Remove(toDelete);
            await _appDbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
