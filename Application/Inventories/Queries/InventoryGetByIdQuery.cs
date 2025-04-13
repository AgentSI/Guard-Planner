using Application.DTOs;
using Application.Interfaces;
using MediatR;

namespace Application.Inventories.Queries
{
    public class InventoryGetByIdQuery(Guid id) : IRequest<InventoryDto>
    {
        public Guid Id { get; set; } = id;
    }

    public class InventoryGetByIdQueryHandler(IAppDbContext appDbContext) : IRequestHandler<InventoryGetByIdQuery, InventoryDto>
    {
        private readonly IAppDbContext _appDbContext = appDbContext;

        public Task<InventoryDto> Handle(InventoryGetByIdQuery request, CancellationToken cancellationToken)
        {
            var inventory = _appDbContext.Inventories.Where(p => p.Id == request.Id).Select(InventoryMapping.InventoryProjection).FirstOrDefault();
            if (inventory != null) return Task.FromResult(inventory);
            else return Task.FromResult(new InventoryDto { });
        }
    }
}
