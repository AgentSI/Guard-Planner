using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Workers.Queries
{
    public class GetWorkerHoursQuery(int month, int year) : IRequest<List<WorkerHoursDto>>
    {
        public int Month { get; set; } = month;
        public int Year { get; set; } = year;
    }

    public class GetWorkerHoursQueryHandler(IAppDbContext appDbContext) : IRequestHandler<GetWorkerHoursQuery, List<WorkerHoursDto>>
    {
        private readonly IAppDbContext _appDbContext = appDbContext;

        public async Task<List<WorkerHoursDto>> Handle(GetWorkerHoursQuery request, CancellationToken cancellationToken)
        {
            var startDate = new DateTime(request.Year, request.Month, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);
            var workersHoursDto = new List<WorkerHoursDto>();
            var workersHours = new List<WorkerHours>();

            var workers = await _appDbContext.Workers.Include(w => w.Guards).ToListAsync(cancellationToken: cancellationToken);

            foreach (var worker in workers)
            {
                double hoursWorked = 0;
                var dailyHours = new List<DailyWorkHoursDto>();

                for (int day = 1; day <= DateTime.DaysInMonth(request.Year, request.Month); day++)
                {
                    var dayDate = new DateTime(request.Year, request.Month, day);
                    var dayOfWeek = dayDate.DayOfWeek;

                    var workHoursForDay = worker.Guards!
                        .Where(wh => wh.Date.Date == dayDate.Date)
                        .Sum(wh => wh.Hours);

                    if (worker.IsWorkDay && workHoursForDay == 0 && (dayOfWeek >= DayOfWeek.Monday && dayOfWeek <= DayOfWeek.Friday))
                    {
                        hoursWorked += 8;
                        dailyHours.Add(new DailyWorkHoursDto
                        {
                            Day = day,
                            HoursWorked = 8
                        });
                    }
                    else
                    {
                        hoursWorked += workHoursForDay;
                        dailyHours.Add(new DailyWorkHoursDto
                        {
                            Day = day,
                            HoursWorked = (int)workHoursForDay
                        });
                    }
                }

                workersHoursDto.Add(new WorkerHoursDto
                {
                    Id = Guid.NewGuid(),
                    WorkerId = worker.Id,
                    WorkerName = worker.Name,
                    Month = request.Month,
                    Year = request.Year,
                    DailyWorkHours = dailyHours.Select(dh => new DailyWorkHoursDto { Day = dh.Day, HoursWorked = dh.HoursWorked }).ToList(),
                    HoursWorked = (int)hoursWorked
                });
            }

            foreach (var workerDto in workersHoursDto)
            {
                var exist = _appDbContext.WorkerHours.Where(w => w.WorkerId == workerDto.WorkerId && w.Month == request.Month && w.Year == request.Year).FirstOrDefault();
                if (exist == null)
                {
                    var workerHoursEntity = new WorkerHours
                    {
                        Id = workerDto.Id,
                        WorkerId = workerDto.WorkerId,
                        Month = workerDto.Month,
                        Year = workerDto.Year,
                        HoursWorked = workerDto.HoursWorked,
                        DailyWorkHours = workerDto.DailyWorkHours!.Select(d => new DailyWorkHours
                        {
                            WorkerHoursId = workerDto.Id,
                            Day = d.Day,
                            HoursWorked = d.HoursWorked
                        }).ToList()
                    };

                    workersHours.Add(workerHoursEntity);
                }
            }

            _appDbContext.WorkerHours.AddRange(workersHours);
            await _appDbContext.SaveChangesAsync(cancellationToken);

            return workersHoursDto;
        }
    }
}
