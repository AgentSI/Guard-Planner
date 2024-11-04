using Application.DTOs;
using Application.Interfaces;
using MediatR;

namespace Application.Receipts.Queries
{
    public class ReceiptGetByIdQuery : IRequest<ReceiptDto>
    {
        public ReceiptGetByIdQuery(Guid id)
        { 
            Id = id;
        }

        public Guid Id { get; set; }
    }

    public class ReceiptGetByIdQueryHandler : IRequestHandler<ReceiptGetByIdQuery, ReceiptDto>
    {
        private readonly IAppDbContext _appDbContext;

        public ReceiptGetByIdQueryHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<ReceiptDto> Handle(ReceiptGetByIdQuery request, CancellationToken cancellationToken)
        {
            var receipt = _appDbContext.Receipts.Where(p => p.Id == request.Id).Select(ReceiptMapping.ReceiptProjection).FirstOrDefault();
            if (receipt != null) return receipt;
            else return null;
        }
    }
}
