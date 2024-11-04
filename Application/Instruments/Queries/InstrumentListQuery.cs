using Application.DTOs;
using Application.Interfaces;
using Application.Interfaces.Pagination;
using MediatR;

namespace Application.Instruments.Queries
{
    public class InstrumentListQuery : IRequest<PaginationResult<InstrumentDto>>
    {
        public InstrumentListQuery(PaginationParameter paginationParameter, Guid receiptId)
        {
            ReceiptId = receiptId;
            PaginationParameter = paginationParameter;
        }

        public Guid ReceiptId { get; set; }
        public PaginationParameter PaginationParameter { get; set; }
    }

    public class InstrumentListQueryHandler : IRequestHandler<InstrumentListQuery, PaginationResult<InstrumentDto>>
    {
        private readonly IPaginationService _paginationService;
        private readonly IAppDbContext _appDbContext;

        public InstrumentListQueryHandler(IPaginationService paginationService, IAppDbContext appDbContext)
        {
            _paginationService = paginationService;
            _appDbContext = appDbContext;
        }

        public async Task<PaginationResult<InstrumentDto>> Handle(InstrumentListQuery request, CancellationToken cancellationToken)
        {
            var query = request.PaginationParameter;
            var list = _appDbContext.Instrument.Where(o => o.ReceiptId == request.ReceiptId).AsQueryable();

            return await _paginationService.PaginateAsync(list, query, InstrumentMapping.InstrumentProjection);
        }
    }
}
