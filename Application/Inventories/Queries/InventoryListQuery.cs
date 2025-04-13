using Application.DTOs;
using Application.Interfaces;
using Application.Interfaces.Pagination;
using MediatR;

namespace Application.Inventories.Queries
{
    public class InventoryListQuery(PaginationParameter paginationParameter, Guid receiptId) : IRequest<PaginationResult<InventoryDto>>
    {
        public Guid ReceiptId { get; set; } = receiptId;
        public PaginationParameter PaginationParameter { get; set; } = paginationParameter;
    }

    public class InventoryListQueryHandler(IPaginationService paginationService, IAppDbContext appDbContext) : IRequestHandler<InventoryListQuery, PaginationResult<InventoryDto>>
    {
        private readonly IPaginationService _paginationService = paginationService;
        private readonly IAppDbContext _appDbContext = appDbContext;

        public async Task<PaginationResult<InventoryDto>> Handle(InventoryListQuery request, CancellationToken cancellationToken)
        {
            var query = request.PaginationParameter;
            var inventories = _appDbContext.Inventories.Where(o => o.ReceiptId == request.ReceiptId).AsQueryable();

            return await _paginationService.PaginateAsync(inventories, query, InventoryMapping.InventoryProjection);
        }
    }
}
