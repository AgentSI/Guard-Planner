using Application.DTOs;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Vacations.Queries
{
    public class VacationsForMonthQuery(int month, int year) : IRequest<List<VacationDto>>
    {
        public int Month { get; set; } = month;
        public int Year { get; set; } = year;
    }

    public class VacationsForMonthQueryHandler(IAppDbContext appDbContext) : IRequestHandler<VacationsForMonthQuery, List<VacationDto>>
    {
        private readonly IAppDbContext _appDbContext = appDbContext;

        public async Task<List<VacationDto>> Handle(VacationsForMonthQuery request, CancellationToken cancellationToken)
        {
            var startOfMonth = new DateTime(request.Year, request.Month, 1);
            var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

            var vacations = await _appDbContext.Vacations
                .Where(v => v.StartDate <= endOfMonth && v.EndDate >= startOfMonth)
                .Select(VacationMapping.VacationProjection)
                .ToListAsync();

            return vacations;
        }
    }
}
