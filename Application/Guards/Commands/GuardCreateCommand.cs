using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Guards.Commands
{
    public class GuardCreateCommand : IRequest<Guid>
    {
        public GuardCreateCommand(GuardDto create)
        {
            Date = create.Date;
            WorkerName = create.WorkerName;
        }

        public DateTime? Date { get; set; }
        public string WorkerName { get; set; }
    }

    public class GuardCreateCommandHandler : IRequestHandler<GuardCreateCommand, Guid>
    {
        private readonly IAppDbContext _appDbContext;

        public GuardCreateCommandHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<Guid> Handle(GuardCreateCommand request, CancellationToken cancellationToken)
        {
            var worker = await _appDbContext.Workers.Where(w => w.Name == request.WorkerName).FirstOrDefaultAsync();
            var existing = _appDbContext.Guards.FirstOrDefault(u => u.Date == request.Date && u.WorkerId == worker.Id);
            if (existing != null) return Guid.Empty;

            var create = new Guard
            {
                Date = request.Date ?? DateTime.Now,
                Hours = IsWeekend(request.Date) ? 24 : 16,
                Worker = worker,
                WorkerId = worker.Id
            };

            _appDbContext.Guards.Add(create);
            await _appDbContext.SaveChangesAsync(cancellationToken);

            return create.Id;
        }

        private bool IsWeekend(DateTime? date)
        {
            return date.Value.DayOfWeek == DayOfWeek.Saturday || date.Value.DayOfWeek == DayOfWeek.Sunday;
        }
    }
}
