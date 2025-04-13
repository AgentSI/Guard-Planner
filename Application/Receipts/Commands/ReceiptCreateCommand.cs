using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Receipts.Commands
{
    public class ReceiptCreateCommand(ReceiptDto create) : IRequest<Guid>
    {
        public string? Name { get; set; } = create.Name;
        public Guid OperationId { get; set; } = create.OperationId;
    }

    public class ReceiptCreateCommandHandler(IAppDbContext appDbContext) : IRequestHandler<ReceiptCreateCommand, Guid>
    {
        private readonly IAppDbContext _appDbContext = appDbContext;

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

            operation!.ReceiptId = create.Id;

            await _appDbContext.SaveChangesAsync(cancellationToken);

            return create.Id;
        }
    }
}
