using Application.DTOs;
using Application.Interfaces;
using Application.Interfaces.Pagination;
using MediatR;

namespace Application.Instruments.Queries
{
    public class InstrumentListQuery(PaginationParameter paginationParameter, Guid receiptId) : IRequest<PaginationResult<InstrumentDto>>
    {
        public Guid ReceiptId { get; set; } = receiptId;
        public PaginationParameter PaginationParameter { get; set; } = paginationParameter;
    }

    public class InstrumentListQueryHandler(IPaginationService paginationService, IAppDbContext appDbContext) : IRequestHandler<InstrumentListQuery, PaginationResult<InstrumentDto>>
    {
        private readonly IPaginationService _paginationService = paginationService;
        private readonly IAppDbContext _appDbContext = appDbContext;

        public async Task<PaginationResult<InstrumentDto>> Handle(InstrumentListQuery request, CancellationToken cancellationToken)
        {
            var query = request.PaginationParameter;
            var list = _appDbContext.Instrument.Where(o => o.ReceiptId == request.ReceiptId).AsQueryable();

            return await _paginationService.PaginateAsync(list, query, InstrumentMapping.InstrumentProjection);
        }
    }
}
