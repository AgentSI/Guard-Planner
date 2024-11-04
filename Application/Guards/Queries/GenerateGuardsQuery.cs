using Application.DTOs;
using Application.Interfaces;
using Application.Vacations.Queries;
using Application.Workers.Queries;
using Domain.Entities;
using MediatR;

namespace Application.Guards.Queries
{
    public class GenerateGuardsQuery : IRequest<List<GuardDto>>
    {
        public GenerateGuardsQuery(int month, int year)
        {
            Month = month;
            Year = year;
        }

        public int Month { get; set; }
        public int Year { get; set; }
    }

    public class GenerateGuardsQueryHandler : IRequestHandler<GenerateGuardsQuery, List<GuardDto>>
    {
        private readonly IMediator _mediator;
        private readonly IAppDbContext _appDbContext;

        public GenerateGuardsQueryHandler(IMediator mediator, IAppDbContext appDbContext)
        {
            _mediator = mediator;
            _appDbContext = appDbContext;
        }

        public async Task<List<GuardDto>> Handle(GenerateGuardsQuery request, CancellationToken cancellationToken)
        {
            var guards = new List<GuardDto>();
            var startOfMonth = new DateTime(request.Year, request.Month, 1);
            var totalDays = DateTime.DaysInMonth(startOfMonth.Year, startOfMonth.Month);
            var workers = await _mediator.Send(new GetWorkersQuery(), cancellationToken);
            var vacations = await _mediator.Send(new VacationsForMonthQuery(request.Month, request.Year), cancellationToken);

            var assignedDates = workers.ToDictionary(worker => worker.Name, worker => new List<DateTime>());
            var allAssignedDates = new HashSet<DateTime>();

            await AssignGuards(request, workers, vacations, assignedDates, allAssignedDates, guards, totalDays, startOfMonth);

            while (HasConsecutiveGuards(assignedDates))
            {
                guards.Clear();
                assignedDates = workers.ToDictionary(worker => worker.Name, worker => new List<DateTime>());
                allAssignedDates.Clear();
                await AssignGuards(request, workers, vacations, assignedDates, allAssignedDates, guards, totalDays, startOfMonth);
            }

            return guards.OrderBy(g => g.WorkerName).ToList();
        }

        private async Task AssignGuards(GenerateGuardsQuery request, List<WorkerDto> workers, List<VacationDto> vacations,
            Dictionary<string, List<DateTime>> assignedDates, HashSet<DateTime> allAssignedDates, List<GuardDto> guards, int totalDays, DateTime startOfMonth)
        {
            foreach (var worker in workers)
            {
                var workerVacations = vacations
                    .Where(v => v.WorkerName.Trim() == worker.Name.Trim())
                    .ToList();

                int assignedCount = assignedDates[worker.Name].Count;

                if (assignedCount >= GetRequiredPercents(worker.Percent))
                {
                    continue;
                }

                var availableDates = Enumerable.Range(1, totalDays)
                    .Select(day => startOfMonth.AddDays(day - 1))
                    .OrderBy(_ => Guid.NewGuid())
                    .ToList();

                foreach (var guardDate in availableDates)
                {
                    if (CanAssignDate(guardDate, assignedDates[worker.Name], allAssignedDates, workerVacations))
                    {
                        guards.Add(new GuardDto
                        {
                            WorkerId = worker.Id,
                            WorkerName = worker.Name,
                            Date = guardDate,
                            Hours = worker.Percent == 1 ? 24 : (IsWeekend(guardDate) ? 24 : 16)
                        });

                        assignedDates[worker.Name].Add(guardDate);
                        allAssignedDates.Add(guardDate);
                        assignedCount++;

                        if (assignedCount >= GetRequiredPercents(worker.Percent))
                        {
                            break;
                        }
                    }
                }
            }

            AssignUnassignedDates(request.Month, request.Year, totalDays, workers, vacations, assignedDates, allAssignedDates, guards);
        }
        private bool IsWeekend(DateTime date)
        {
            return date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;
        }

        private bool HasConsecutiveGuards(Dictionary<string, List<DateTime>> assignedDates)
        {
            foreach (var workerAssignments in assignedDates.Values)
            {
                if (workerAssignments.OrderBy(date => date).Zip(workerAssignments.OrderBy(date => date).Skip(1), (prev, next) => (prev, next))
                    .Any(pair => (pair.next - pair.prev).TotalDays == 1))
                {
                    return true;
                }
            }
            return false;
        }

        private void AssignUnassignedDates(int month, int year, int totalDays, List<WorkerDto> workers,
            List<VacationDto> vacations, Dictionary<string, List<DateTime>> assignedDates,
            HashSet<DateTime> allAssignedDates, List<GuardDto> guards)
        {
            for (int day = 1; day <= totalDays; day++)
            {
                var date = new DateTime(year, month, day);
                if (!allAssignedDates.Contains(date))
                {
                    var availableWorkers = workers.Where(worker =>
                    {
                        var workerVacations = vacations
                            .Where(v => v.WorkerName.Trim() == worker.Name.Trim())
                            .ToList();

                        return !workerVacations.Any(v => date >= v.StartDate && date <= v.EndDate) &&
                               assignedDates[worker.Name].Count < GetRequiredPercents(worker.Percent) &&
                               !assignedDates[worker.Name].Any(assignedDate => Math.Abs((assignedDate - date).TotalDays) < 1);
                    }).ToList();

                    if (availableWorkers.Any())
                    {
                        var workerToAssign = availableWorkers.First();
                        guards.Add(new GuardDto
                        {
                            WorkerId = workerToAssign.Id,
                            WorkerName = workerToAssign.Name,
                            Date = date,
                            Hours = workerToAssign.Percent == 1 ? 24 : (IsWeekend(date) ? 24 : 16)
                        });

                        assignedDates[workerToAssign.Name].Add(date);
                        allAssignedDates.Add(date);
                    }
                }
            }
        }

        private bool CanAssignDate(DateTime guardDate, List<DateTime> workerAssignedDates, HashSet<DateTime> allAssignedDates, List<VacationDto> workerVacations)
        {
            if (allAssignedDates.Contains(guardDate))
            {
                return false;
            }

            bool hasConsecutiveAssignment = workerAssignedDates.Any(date => Math.Abs((date - guardDate).TotalDays) == 1);
            if (hasConsecutiveAssignment)
            {
                return false;
            }

            bool hasGap = workerAssignedDates.Any(date => Math.Abs((date - guardDate).TotalDays) < 4);
            if (hasGap)
            {
                return false;
            }

            if (workerAssignedDates.Any(date => Math.Abs((date - guardDate).TotalDays) == 6 ||
                                                Math.Abs((date - guardDate).TotalDays) == -6 ||
                                                Math.Abs((date - guardDate).TotalDays) == 7 ||
                                                Math.Abs((date - guardDate).TotalDays) == -7 ||
                                                Math.Abs((date - guardDate).TotalDays) == 8 ||
                                                Math.Abs((date - guardDate).TotalDays) == -8 ||
                                                Math.Abs((date - guardDate).TotalDays) == 14 ||
                                                Math.Abs((date - guardDate).TotalDays) == -14 ||
                                                Math.Abs((date - guardDate).TotalDays) == 21 ||
                                                Math.Abs((date - guardDate).TotalDays) == -21))
            {
                return false;
            }

            bool isOnVacation = workerVacations.Any(v => guardDate >= v.StartDate && guardDate <= v.EndDate);
            if (isOnVacation)
            {
                return false;
            }

            return true;
        }

        private int GetRequiredPercents(double guardPercentage)
        {
            return (int)(guardPercentage * 8);
        }
    }
}