using Application.DTOs;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Instruments.Commands
{
    public class InstrumentEditCommand(InstrumentDto model) : IRequest<Unit>
    {
        public Guid Id { get; set; } = model.Id;
        public int Amount { get; set; } = model.Amount;
        public string Name { get; set; } = model.Name;
        public Guid ReceiptId { get; set; } = model.ReceiptId;
    }

    public class InstrumentEditCommandHandler(IAppDbContext appDbContext) : IRequestHandler<InstrumentEditCommand, Unit>
    {
        private readonly IAppDbContext _appDbContext = appDbContext;

        public async Task<Unit> Handle(InstrumentEditCommand request, CancellationToken cancellationToken)
        {
            var toEdit = await _appDbContext.Instrument.Where(p => p.Id == request.Id).FirstOrDefaultAsync();

            if (toEdit != null)
            {
                toEdit.Name = request.Name;
                toEdit.Amount = request.Amount;
                toEdit.ReceiptId = request.ReceiptId;
            }

            await _appDbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
