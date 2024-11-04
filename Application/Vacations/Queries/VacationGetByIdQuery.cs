using Application.DTOs;
using Application.Interfaces;
using MediatR;

namespace Application.Vacations.Queries
{
    public class VacationGetByIdQuery : IRequest<VacationDto>
    {
        public VacationGetByIdQuery(Guid id)
        { 
            Id = id;
        }

        public Guid Id { get; set; }
    }

    public class VacationGetByIdQueryHandler : IRequestHandler<VacationGetByIdQuery, VacationDto>
    {
        private readonly IAppDbContext _appDbContext;

        public VacationGetByIdQueryHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<VacationDto> Handle(VacationGetByIdQuery request, CancellationToken cancellationToken)
        {
            var vacation = _appDbContext.Vacations.Where(p => p.Id == request.Id).Select(VacationMapping.VacationProjection).FirstOrDefault();
            if (vacation != null) return vacation;
            else return null;
        }
    }
}
