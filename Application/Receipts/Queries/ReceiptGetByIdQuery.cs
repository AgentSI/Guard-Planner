using Application.DTOs;
using Application.Interfaces;
using MediatR;

namespace Application.Receipts.Queries
{
    public class ReceiptGetByIdQuery(Guid id) : IRequest<ReceiptDto>
    {
        public Guid Id { get; set; } = id;
    }

    public class ReceiptGetByIdQueryHandler(IAppDbContext appDbContext) : IRequestHandler<ReceiptGetByIdQuery, ReceiptDto>
    {
        private readonly IAppDbContext _appDbContext = appDbContext;

        public Task<ReceiptDto> Handle(ReceiptGetByIdQuery request, CancellationToken cancellationToken)
        {
            var receipt = _appDbContext.Receipts.Where(p => p.Id == request.Id).Select(ReceiptMapping.ReceiptProjection).FirstOrDefault();
            if (receipt != null) return Task.FromResult(receipt);
            else return Task.FromResult(new ReceiptDto { });
        }
    }
}
