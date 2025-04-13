using Application.DTOs;
using Application.Interfaces;
using Application.Workers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Hours.Queries
{
    public class GetHourListQuery : IRequest<List<HourDto>>
    {
        public GetHourListQuery() { }
    }

    public class GetHourListQueryHandler(IAppDbContext appDbContext) : IRequestHandler<GetHourListQuery, List<HourDto>>
    {
        private readonly IAppDbContext _appDbContext = appDbContext;

        public async Task<List<HourDto>> Handle(GetHourListQuery request, CancellationToken cancellationToken)
        {
            var hours = await _appDbContext.Hours
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            return hours.Select(hour => HourMapping.HourProjection(hour)).ToList();
        }
    }
}
