using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Instruments.Commands
{
    public class InstrumentDeleteCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
    }

    public class InstrumentDeleteCommandHandler(IAppDbContext appDbContext) : IRequestHandler<InstrumentDeleteCommand, Unit>
    {
        private readonly IAppDbContext _appDbContext = appDbContext;

        public async Task<Unit> Handle(InstrumentDeleteCommand request, CancellationToken cancellationToken)
        {
            var toDelete = await _appDbContext.Instrument.Where(e => e.Id == request.Id).FirstOrDefaultAsync();

            _appDbContext.Instrument.Remove(toDelete!);
            await _appDbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
