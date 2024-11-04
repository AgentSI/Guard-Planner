using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Instruments.Commands
{
    public class InstrumentCreateCommand : IRequest<Guid>
    {
        public InstrumentCreateCommand(InstrumentDto create)
        {
            Amount = create.Amount;
            Name = create.Name;
            ReceiptId = create.ReceiptId;
        }

        public int Amount { get; set; }
        public string Name { get; set; }
        public Guid ReceiptId { get; set; }
    }

    public class InstrumentCreateCommandHandler : IRequestHandler<InstrumentCreateCommand, Guid>
    {
        private readonly IAppDbContext _appDbContext;

        public InstrumentCreateCommandHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<Guid> Handle(InstrumentCreateCommand request, CancellationToken cancellationToken)
        {
            var create = new Instrument
            {
                Amount = request.Amount,
                Name = request.Name,
                ReceiptId = request.ReceiptId
            };

            _appDbContext.Instrument.Add(create);
            await _appDbContext.SaveChangesAsync(cancellationToken);

            return create.Id;
        }
    }
}
