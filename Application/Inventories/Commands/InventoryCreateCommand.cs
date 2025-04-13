using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Inventories.Commands
{
    public class InventoryCreateCommand(InventoryDto create) : IRequest<Guid>
    {
        public decimal Amount { get; set; } = create.Amount;
        public string? Name { get; set; } = create.Name;
        public string? Measure { get; set; } = create.Measure;
        public Guid ReceiptId { get; set; } = create.ReceiptId;
    }

    public class InventoryCreateCommandHandler(IAppDbContext appDbContext) : IRequestHandler<InventoryCreateCommand, Guid>
    {
        private readonly IAppDbContext _appDbContext = appDbContext;

        public async Task<Guid> Handle(InventoryCreateCommand request, CancellationToken cancellationToken)
        {
            var create = new Inventory
            {
                Amount = request.Amount,
                Name = request.Name,
                ReceiptId = request.ReceiptId,
                Measure = request.Measure
            };

            _appDbContext.Inventories.Add(create);
            await _appDbContext.SaveChangesAsync(cancellationToken);

            return create.Id;
        }
    }
}
