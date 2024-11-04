using Application.DTOs;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Workers.Queries
{
    public class GetWorkerHoursQuery : IRequest<List<WorkerHoursDto>>
    {
        public GetWorkerHoursQuery(int month, int year) 
        {
            Month = month;
            Year = year;
        }

        public int Month { get; set; }
        public int Year { get; set; }
    }

    public class GetWorkerHoursQueryHandler : IRequestHandler<GetWorkerHoursQuery, List<WorkerHoursDto>>
    {
        private readonly IAppDbContext _appDbContext;
        public GetWorkerHoursQueryHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<List<WorkerHoursDto>> Handle(GetWorkerHoursQuery request, CancellationToken cancellationToken)
        {
            var startDate = new DateTime(request.Year, request.Month, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);

            var workersHours = await _appDbContext.Workers
                .Where(w => w.Guards.Any(wh => wh.Date >= startDate && wh.Date <= endDate))
                .Select(w => new WorkerHoursDto
                {
                    WorkerName = w.Name,
                    Month = request.Month,
                    Year = request.Year,
                    HoursWorked = (int)w.Guards
                        .Where(wh => wh.Date >= startDate && wh.Date <= endDate)
                        .Sum(wh => wh.Hours)
                }).ToListAsync();

            return workersHours;
        }
    }
}
