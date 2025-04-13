using Application.DTOs;
using Application.Interfaces;
using MediatR;

namespace Application.Instruments.Queries
{
    public class InstrumentGetByIdQuery(Guid id) : IRequest<InstrumentDto>
    {
        public Guid Id { get; set; } = id;
    }

    public class InstrumentGetByIdQueryHandler(IAppDbContext appDbContext) : IRequestHandler<InstrumentGetByIdQuery, InstrumentDto>
    {
        private readonly IAppDbContext _appDbContext = appDbContext;

        public Task<InstrumentDto> Handle(InstrumentGetByIdQuery request, CancellationToken cancellationToken)
        {
            var instrument = _appDbContext.Instrument.Where(p => p.Id == request.Id).Select(InstrumentMapping.InstrumentProjection).FirstOrDefault();
            if (instrument != null) return Task.FromResult(instrument);
            else return Task.FromResult(new InstrumentDto { });
        }
    }
}
