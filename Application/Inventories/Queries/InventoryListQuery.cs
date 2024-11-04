using Application.DTOs;
using Application.Interfaces;
using Application.Interfaces.Pagination;
using MediatR;

namespace Application.Inventories.Queries
{
    public class InventoryListQuery : IRequest<PaginationResult<InventoryDto>>
    {
        public InventoryListQuery(PaginationParameter paginationParameter, Guid receiptId)
        {
            ReceiptId = receiptId;
            PaginationParameter = paginationParameter;
        }

        public Guid ReceiptId { get; set; }
        public PaginationParameter PaginationParameter { get; set; }
    }

    public class InventoryListQueryHandler : IRequestHandler<InventoryListQuery, PaginationResult<InventoryDto>>
    {
        private readonly IPaginationService _paginationService;
        private readonly IAppDbContext _appDbContext;

        public InventoryListQueryHandler(IPaginationService paginationService, IAppDbContext appDbContext)
        {
            _paginationService = paginationService;
            _appDbContext = appDbContext;
        }

        public async Task<PaginationResult<InventoryDto>> Handle(InventoryListQuery request, CancellationToken cancellationToken)
        {
            var query = request.PaginationParameter;
            var inventories = _appDbContext.Inventories.Where(o => o.ReceiptId == request.ReceiptId).AsQueryable();

            return await _paginationService.PaginateAsync(inventories, query, InventoryMapping.InventoryProjection);
        }
    }
}
