using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Receipts.Commands
{
    public class ReceiptCreateCommand : IRequest<Guid>
    {
        public ReceiptCreateCommand(ReceiptDto create)
        {
            Name = create.Name;
            OperationId = create.OperationId;
        }

        public string Name { get; set; }
        public Guid OperationId { get; set; }
    }

    public class ReceiptCreateCommandHandler : IRequestHandler<ReceiptCreateCommand, Guid>
    {
        private readonly IAppDbContext _appDbContext;

        public ReceiptCreateCommandHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<Guid> Handle(ReceiptCreateCommand request, CancellationToken cancellationToken)
        {
            var operation = await _appDbContext.Operations.Where(o => o.Id == request.OperationId).FirstOrDefaultAsync();
            var create = new Receipt
            {
                Name = request.Name,
                OperationId = request.OperationId,
                Operation = operation
            };

            _appDbContext.Receipts.Add(create);

            operation.ReceiptId = create.Id;

            await _appDbContext.SaveChangesAsync(cancellationToken);

            return create.Id;
        }
    }
}
