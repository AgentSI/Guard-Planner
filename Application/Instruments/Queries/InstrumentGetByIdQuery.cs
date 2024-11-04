using Application.DTOs;
using Application.Interfaces;
using MediatR;

namespace Application.Instruments.Queries
{
    public class InstrumentGetByIdQuery : IRequest<InstrumentDto>
    {
        public InstrumentGetByIdQuery(Guid id)
        { 
            Id = id;
        }

        public Guid Id { get; set; }
    }

    public class InstrumentGetByIdQueryHandler : IRequestHandler<InstrumentGetByIdQuery, InstrumentDto>
    {
        private readonly IAppDbContext _appDbContext;

        public InstrumentGetByIdQueryHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<InstrumentDto> Handle(InstrumentGetByIdQuery request, CancellationToken cancellationToken)
        {
            var instrument = _appDbContext.Instrument.Where(p => p.Id == request.Id).Select(InstrumentMapping.InstrumentProjection).FirstOrDefault();
            if (instrument != null) return instrument;
            else return null;
        }
    }
}
