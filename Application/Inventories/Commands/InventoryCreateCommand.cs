using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Inventories.Commands
{
    public class InventoryCreateCommand : IRequest<Guid>
    {
        public InventoryCreateCommand(InventoryDto create)
        {
            Amount = create.Amount;
            Name = create.Name;
            ReceiptId = create.ReceiptId;
            Measure = create.Measure;
        }

        public decimal Amount { get; set; }
        public string Name { get; set; }
        public string? Measure { get; set; }
        public Guid ReceiptId { get; set; }
    }

    public class InventoryCreateCommandHandler : IRequestHandler<InventoryCreateCommand, Guid>
    {
        private readonly IAppDbContext _appDbContext;

        public InventoryCreateCommandHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

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
