using Application.DTOs;
using Application.Interfaces;
using MediatR;

namespace Application.Guards.Queries
{
    public class GuardGetByIdQuery : IRequest<GuardDto>
    {
        public GuardGetByIdQuery(Guid id)
        { 
            Id = id;
        }

        public Guid Id { get; set; }
    }

    public class GuardGetByIdQueryHandler : IRequestHandler<GuardGetByIdQuery, GuardDto>
    {
        private readonly IAppDbContext _appDbContext;

        public GuardGetByIdQueryHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<GuardDto> Handle(GuardGetByIdQuery request, CancellationToken cancellationToken)
        {
            var guard = _appDbContext.Guards.Where(p => p.Id == request.Id).Select(GuardMapping.GuardProjection).FirstOrDefault();
            if (guard != null) return guard;
            else return null;
        }
    }
}
