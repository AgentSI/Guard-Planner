using Application.DTOs;
using Application.Interfaces;
using MediatR;

namespace Application.Guards.Queries
{
    public class GuardGetByIdQuery(Guid id) : IRequest<GuardDto>
    {
        public Guid Id { get; set; } = id;
    }

    public class GuardGetByIdQueryHandler(IAppDbContext appDbContext) : IRequestHandler<GuardGetByIdQuery, GuardDto>
    {
        private readonly IAppDbContext _appDbContext = appDbContext;

        public Task<GuardDto> Handle(GuardGetByIdQuery request, CancellationToken cancellationToken)
        {
            var guard = _appDbContext.Guards.Where(p => p.Id == request.Id).Select(GuardMapping.GuardProjection).FirstOrDefault();
            if (guard != null) return Task.FromResult(guard);
            else return Task.FromResult(new GuardDto { });
        }
    }
}
