using Application.DTOs;
using Application.Interfaces;
using MediatR;

namespace Application.Vacations.Queries
{
    public class VacationGetByIdQuery(Guid id) : IRequest<VacationDto>
    {
        public Guid Id { get; set; } = id;
    }

    public class VacationGetByIdQueryHandler(IAppDbContext appDbContext) : IRequestHandler<VacationGetByIdQuery, VacationDto>
    {
        private readonly IAppDbContext _appDbContext = appDbContext;

        public Task<VacationDto> Handle(VacationGetByIdQuery request, CancellationToken cancellationToken)
        {
            var vacation = _appDbContext.Vacations.Where(p => p.Id == request.Id).Select(VacationMapping.VacationProjection).FirstOrDefault();
            if (vacation != null) return Task.FromResult(vacation);
            else return Task.FromResult(new VacationDto());
        }
    }
}
