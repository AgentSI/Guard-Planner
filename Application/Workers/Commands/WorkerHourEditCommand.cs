using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Workers.Commands
{
    public class WorkerHourEditCommand(WorkerHoursDto model, int day, int hours) : IRequest<Unit>
    {
        public Guid Id { get; set; } = model.Id;
        public Guid WorkerId { get; set; } = model.WorkerId;
        public int Month { get; set; } = model.Month;
        public int Year { get; set; } = model.Year;
        public int Day { get; set; } = day;
        public int HoursWorked { get; set; } = hours;
    }

    public class WorkerHourEditCommandHandler(IAppDbContext appDbContext) : IRequestHandler<WorkerHourEditCommand, Unit>
    {
        private readonly IAppDbContext _appDbContext = appDbContext;

        public async Task<Unit> Handle(WorkerHourEditCommand request, CancellationToken cancellationToken)
        {
            var workerHours = await _appDbContext.WorkerHours
                .Include(w => w.DailyWorkHours)
                .FirstOrDefaultAsync(w => w.WorkerId == request.WorkerId && w.Month == request.Month && w.Year == request.Year, cancellationToken);

            if (workerHours != null)
            {
                if (workerHours.DailyWorkHours != null)
                {
                    var dailyWorkHours = workerHours.DailyWorkHours.FirstOrDefault(d => d.Day == request.Day);
                    if (dailyWorkHours != null)
                    {
                        dailyWorkHours.HoursWorked = request.HoursWorked;
                    }
                    else
                    {
                        workerHours.DailyWorkHours.Add(new DailyWorkHours
                        {
                            Id = Guid.NewGuid(),
                            WorkerHoursId = workerHours.Id,
                            Day = request.Day,
                            HoursWorked = request.HoursWorked
                        });
                    }
                }
                else
                {
                    workerHours.DailyWorkHours =
                    [
                        new DailyWorkHours
                        {
                            Id = Guid.NewGuid(),
                            WorkerHoursId = workerHours.Id,
                            Day = request.Day,
                            HoursWorked = request.HoursWorked
                        }
                    ];
                }
            }

            await _appDbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
