using Application.DTOs;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Guards.Queries
{
    public class GuardsGetQuery(int month, int year) : IRequest<List<GuardDto>>
    {
        public int Month { get; set; } = month;
        public int Year { get; set; } = year;
    }

    public class GuardsGetQueryHandler(IAppDbContext appDbContext) : IRequestHandler<GuardsGetQuery, List<GuardDto>>
    {
        private readonly IAppDbContext _appDbContext = appDbContext;

        public async Task<List<GuardDto>> Handle(GuardsGetQuery request, CancellationToken cancellationToken)
        {
            var guards = await _appDbContext.Guards
                .Where(o => o.Date.Month == request.Month && o.Date.Year == request.Year)
                .Select(GuardMapping.GuardProjection)
                .ToListAsync();

            return guards;
        }
    }
}
