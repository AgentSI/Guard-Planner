using Application.DTOs;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Vacations.Queries
{
    public class VacationsForMonthQuery : IRequest<List<VacationDto>>
    {
        public VacationsForMonthQuery(int month, int year)
        {
            Month = month;
            Year = year;
        }

        public int Month { get; set; }
        public int Year { get; set; }
    }

    public class VacationsForMonthQueryHandler : IRequestHandler<VacationsForMonthQuery, List<VacationDto>>
    {
        private readonly IAppDbContext _appDbContext;

        public VacationsForMonthQueryHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

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
