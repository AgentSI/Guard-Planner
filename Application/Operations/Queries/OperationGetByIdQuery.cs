using Application.DTOs;
using Application.Interfaces;
using MediatR;

namespace Application.Operations.Queries
{
    public class OperationGetByIdQuery : IRequest<OperationDto>
    {
        public OperationGetByIdQuery(Guid id)
        { 
            Id = id;
        }

        public Guid Id { get; set; }
    }

    public class OperationGetByIdQueryHandler : IRequestHandler<OperationGetByIdQuery, OperationDto>
    {
        private readonly IAppDbContext _appDbContext;

        public OperationGetByIdQueryHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<OperationDto> Handle(OperationGetByIdQuery request, CancellationToken cancellationToken)
        {
            var operation = _appDbContext.Operations.Where(p => p.Id == request.Id).Select(OperationMapping.OperationProjection).FirstOrDefault();
            if (operation != null) return operation;
            else return null;
        }
    }
}
