using Application.DTOs;
using Application.Interfaces;
using MediatR;

namespace Application.Operations.Queries
{
    public class OperationGetByIdQuery(Guid id) : IRequest<OperationDto>
    {
        public Guid Id { get; set; } = id;
    }

    public class OperationGetByIdQueryHandler(IAppDbContext appDbContext) : IRequestHandler<OperationGetByIdQuery, OperationDto>
    {
        private readonly IAppDbContext _appDbContext = appDbContext;

        public Task<OperationDto> Handle(OperationGetByIdQuery request, CancellationToken cancellationToken)
        {
            var operation = _appDbContext.Operations.Where(p => p.Id == request.Id).Select(OperationMapping.OperationProjection).FirstOrDefault();
            if (operation != null) return Task.FromResult(operation);
            else return Task.FromResult(new OperationDto { });
        }
    }
}
