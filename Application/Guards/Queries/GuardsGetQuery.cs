using Application.DTOs;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Guards.Queries
{
    public class GuardsGetQuery : IRequest<List<GuardDto>>
    {
        public GuardsGetQuery(int month, int year)
        {
            Month = month;
            Year = year;
        }

        public int Month { get; set; }
        public int Year { get; set; }
    }

    public class GuardsGetQueryHandler : IRequestHandler<GuardsGetQuery, List<GuardDto>>
    {
        private readonly IAppDbContext _appDbContext;

        public GuardsGetQueryHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

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
