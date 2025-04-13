using Application.DTOs;
using Application.Vacations.Queries;
using Application.Workers.Queries;
using MediatR;

namespace Application.Guards.Queries
{
    public class GenerateGuardsQuery(int month, int year) : IRequest<List<GuardDto>>
    {
        public int Month { get; set; } = month;
        public int Year { get; set; } = year;
    }

    public class GenerateGuardsQueryHandler(IMediator mediator) : IRequestHandler<GenerateGuardsQuery, List<GuardDto>>
    {
        private readonly IMediator _mediator = mediator;

        public async Task<List<GuardDto>> Handle(GenerateGuardsQuery request, CancellationToken cancellationToken)
        {
            var guards = new List<GuardDto>();
            var startOfMonth = new DateTime(request.Year, request.Month, 1);
            var totalDays = DateTime.DaysInMonth(startOfMonth.Year, startOfMonth.Month);
            var workers = await _mediator.Send(new GetWorkersQuery(), cancellationToken);

            var previousMonth = startOfMonth.AddDays(-1);
            var lastGuardFromPreviousMonth = await _mediator.Send(
                new GuardsGetQuery(previousMonth.Month, previousMonth.Year), cancellationToken);
            
            var lastWorkerFromPreviousMonth = lastGuardFromPreviousMonth
                .Where(g => g.Date?.Date == previousMonth.Date)
                .Select(g => g.WorkerName)
                .FirstOrDefault();

            if (lastWorkerFromPreviousMonth != null)
            {
                var currentIndex = workers.FindIndex(w => 
                {
                    var fullName = (w.Name + " " + w.FirstName).Trim();
                    return fullName == lastWorkerFromPreviousMonth.Trim();
                });

                if (currentIndex >= 0)
                {
                    if (currentIndex == workers.Count - 1)
                    {
                        workers = workers.ToList();
                    }
                    else
                    {
                        var reorderedWorkers = workers
                            .Skip(currentIndex + 1)
                            .Concat(workers.Take(currentIndex + 1))
                            .ToList();
                        workers = reorderedWorkers;
                    }
                }
            }

            var vacations = await _mediator.Send(new VacationsForMonthQuery(request.Month, request.Year), cancellationToken);
            var assignedDates = workers.ToDictionary(
                worker => (worker.Name + " " + worker.FirstName).Trim(),
                worker => new List<DateTime>()
            );
            var allAssignedDates = new HashSet<DateTime>();

            AssignGuards(request, workers, vacations, assignedDates, allAssignedDates, guards, totalDays, startOfMonth, 3);

            if (allAssignedDates.Count < totalDays)
            {
                guards.Clear();
                assignedDates = workers.ToDictionary(
                    worker => (worker.Name + " " + worker.FirstName).Trim(),
                    worker => new List<DateTime>()
                );
                allAssignedDates.Clear();
                AssignGuards(request, workers, vacations, assignedDates, allAssignedDates, guards, totalDays, startOfMonth, 2);
            }

            if (allAssignedDates.Count < totalDays)
            {
                guards.Clear();
                assignedDates = workers.ToDictionary(
                    worker => (worker.Name + " " + worker.FirstName).Trim(),
                    worker => new List<DateTime>()
                );
                allAssignedDates.Clear();
                AssignGuards(request, workers, vacations, assignedDates, allAssignedDates, guards, totalDays, startOfMonth, 1);
            }

            return guards.OrderBy(g => g.WorkerName).ToList();
        }

        private void AssignGuards(GenerateGuardsQuery request, List<WorkerDto> workers, List<VacationDto> vacations,
            Dictionary<string, List<DateTime>> assignedDates, HashSet<DateTime> allAssignedDates, List<GuardDto> guards, 
            int totalDays, DateTime startOfMonth, int minDaysBetweenGuards)
        {
            var dates = Enumerable.Range(1, totalDays)
                .Select(day => startOfMonth.AddDays(day - 1))
                .OrderBy(d => d)
                .ToList();

            foreach (var date in dates)
            {
                if (allAssignedDates.Contains(date)) continue;

                var availableWorkers = workers.Where(worker =>
                {
                    var fullName = (worker.Name + " " + worker.FirstName).Trim();
                    var workerVacations = vacations
                        .Where(v => v.WorkerName!.Trim() == fullName)
                        .ToList();

                    return !workerVacations.Any(v => date >= v.StartDate && date <= v.EndDate) &&
                           assignedDates[fullName].Count < GetRequiredPercents(worker.Percent) &&
                           !assignedDates[fullName].Any(assignedDate => Math.Abs((assignedDate - date).TotalDays) <= minDaysBetweenGuards);
                })
                .OrderBy(w => assignedDates[(w.Name + " " + w.FirstName).Trim()].Count)
                .ToList();

                if (availableWorkers.Any())
                {
                    var workerToAssign = availableWorkers.First();
                    guards.Add(new GuardDto
                    {
                        WorkerId = workerToAssign.Id,
                        WorkerName = workerToAssign.Name + " " + workerToAssign.FirstName,
                        Date = date,
                        Hours = workerToAssign.Percent == 1 ? 24 : (IsWeekend(date) ? 24 : 16)
                    });

                    var fullName = (workerToAssign.Name + " " + workerToAssign.FirstName).Trim();
                    assignedDates[fullName].Add(date);
                    allAssignedDates.Add(date);
                }
            }
        }

        private bool IsWeekend(DateTime date)
        {
            return date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;
        }

        private static int GetRequiredPercents(double guardPercentage)
        {
            return (int)(guardPercentage * 8);
        }
    }
}
