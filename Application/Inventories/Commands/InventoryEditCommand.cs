using Application.DTOs;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Inventories.Commands
{
    public class InventoryEditCommand(InventoryDto model) : IRequest<Unit>
    {
        public Guid Id { get; set; } = model.Id;
        public decimal Amount { get; set; } = model.Amount;
        public string? Name { get; set; } = model.Name;
        public string? Measure { get; set; } = model.Measure;
        public Guid ReceiptId { get; set; } = model.ReceiptId;
    }

    public class InventoryEditCommandHandler(IAppDbContext appDbContext) : IRequestHandler<InventoryEditCommand, Unit>
    {
        private readonly IAppDbContext _appDbContext = appDbContext;

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
