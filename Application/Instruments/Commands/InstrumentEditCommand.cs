using Application.DTOs;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Instruments.Commands
{
    public class InstrumentEditCommand : IRequest<Unit>
    {
        public InstrumentEditCommand(InstrumentDto model)
        {
            Id = model.Id;
            Amount = model.Amount;
            Name = model.Name;
            ReceiptId = model.ReceiptId;
        }

        public Guid Id { get; set; }
        public int Amount { get; set; }
        public string Name { get; set; }
        public Guid ReceiptId { get; set; }
    }

    public class InstrumentEditCommandHandler : IRequestHandler<InstrumentEditCommand, Unit>
    {
        private readonly IAppDbContext _appDbContext;

        public InstrumentEditCommandHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

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
