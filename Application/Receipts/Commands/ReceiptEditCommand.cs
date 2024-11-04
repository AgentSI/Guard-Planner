using Application.DTOs;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Receipts.Commands
{
    public class ReceiptEditCommand : IRequest<Unit>
    {
        public ReceiptEditCommand(ReceiptDto model)
        {
            Id = model.Id;
            Name = model.Name;
            OperationId = model.OperationId;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid OperationId { get; set; }
    }

    public class ReceiptEditCommandHandler : IRequestHandler<ReceiptEditCommand, Unit>
    {
        private readonly IAppDbContext _appDbContext;

        public ReceiptEditCommandHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

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
