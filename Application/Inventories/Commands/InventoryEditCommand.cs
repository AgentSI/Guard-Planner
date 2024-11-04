using Application.DTOs;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Inventories.Commands
{
    public class InventoryEditCommand : IRequest<Unit>
    {
        public InventoryEditCommand(InventoryDto model)
        {
            Id = model.Id;
            Amount = model.Amount;
            Name = model.Name;
            ReceiptId = model.ReceiptId;
            Measure = model.Measure;
        }

        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public string Name { get; set; }
        public string? Measure { get; set; }
        public Guid ReceiptId { get; set; }
    }

    public class InventoryEditCommandHandler : IRequestHandler<InventoryEditCommand, Unit>
    {
        private readonly IAppDbContext _appDbContext;

        public InventoryEditCommandHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<Unit> Handle(InventoryEditCommand request, CancellationToken cancellationToken)
        {
            var toEdit = await _appDbContext.Inventories.Where(p => p.Id == request.Id).FirstOrDefaultAsync();

            if (toEdit != null)
            {
                toEdit.Name = request.Name;
                toEdit.Amount = request.Amount;
                toEdit.ReceiptId = request.ReceiptId;
                toEdit.Measure = request.Measure;
            }

            await _appDbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
