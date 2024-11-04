using Application.DTOs;
using Application.Interfaces;
using MediatR;

namespace Application.Inventories.Queries
{
    public class InventoryGetByIdQuery : IRequest<InventoryDto>
    {
        public InventoryGetByIdQuery(Guid id)
        { 
            Id = id;
        }

        public Guid Id { get; set; }
    }

    public class InventoryGetByIdQueryHandler : IRequestHandler<InventoryGetByIdQuery, InventoryDto>
    {
        private readonly IAppDbContext _appDbContext;

        public InventoryGetByIdQueryHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<InventoryDto> Handle(InventoryGetByIdQuery request, CancellationToken cancellationToken)
        {
            var inventory = _appDbContext.Inventories.Where(p => p.Id == request.Id).Select(InventoryMapping.InventoryProjection).FirstOrDefault();
            if (inventory != null) return inventory;
            else return null;
        }
    }
}
