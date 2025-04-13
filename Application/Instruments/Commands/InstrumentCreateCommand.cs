using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Instruments.Commands
{
    public class InstrumentCreateCommand(InstrumentDto create) : IRequest<Guid>
    {
        public int Amount { get; set; } = create.Amount;
        public string Name { get; set; } = create.Name;
        public Guid ReceiptId { get; set; } = create.ReceiptId;
    }

    public class InstrumentCreateCommandHandler(IAppDbContext appDbContext) : IRequestHandler<InstrumentCreateCommand, Guid>
    {
        private readonly IAppDbContext _appDbContext = appDbContext;

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
