using Application.DTOs;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Receipts.Commands
{
    public class ReceiptEditCommand(ReceiptDto model) : IRequest<Unit>
    {
        public Guid Id { get; set; } = model.Id;
        public string? Name { get; set; } = model.Name;
        public Guid OperationId { get; set; } = model.OperationId;
    }

    public class ReceiptEditCommandHandler(IAppDbContext appDbContext) : IRequestHandler<ReceiptEditCommand, Unit>
    {
        private readonly IAppDbContext _appDbContext = appDbContext;

        public async Task<Unit> Handle(ReceiptEditCommand request, CancellationToken cancellationToken)
        {
            var toEdit = await _appDbContext.Receipts.Where(p => p.Id == request.Id).FirstOrDefaultAsync();

            if (toEdit != null)
            {
                toEdit.Name = request.Name;
                toEdit.OperationId = request.OperationId;
            }

            await _appDbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
