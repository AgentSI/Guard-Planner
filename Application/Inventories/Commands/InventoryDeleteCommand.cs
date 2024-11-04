using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Inventories.Commands
{
    public class InventoryDeleteCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
    }

    public class InventoryDeleteCommandHandler : IRequestHandler<InventoryDeleteCommand, Unit>
    {
        private readonly IAppDbContext _appDbContext;

        public InventoryDeleteCommandHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<Unit> Handle(InventoryDeleteCommand request, CancellationToken cancellationToken)
        {
            var toDelete = await _appDbContext.Inventories.Where(e => e.Id == request.Id).FirstOrDefaultAsync();

            _appDbContext.Inventories.Remove(toDelete);
            await _appDbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
