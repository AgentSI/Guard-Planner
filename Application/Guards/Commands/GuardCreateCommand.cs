using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Guards.Commands
{
    public class GuardCreateCommand(GuardDto create) : IRequest<Guid>
    {
        public DateTime? Date { get; set; } = create.Date;
        public string? WorkerName { get; set; } = create.WorkerName;
    }

    public class GuardCreateCommandHandler(IAppDbContext appDbContext) : IRequestHandler<GuardCreateCommand, Guid>
    {
        private readonly IAppDbContext _appDbContext = appDbContext;

        public async Task<Guid> Handle(GuardCreateCommand request, CancellationToken cancellationToken)
        {
            var worker = await _appDbContext.Workers.Where(w => w.Name == request.WorkerName).FirstOrDefaultAsync(cancellationToken: cancellationToken);
            var existing = _appDbContext.Guards.FirstOrDefault(u => u.Date == request.Date && u.WorkerId == worker!.Id);
            if (existing != null) return Guid.Empty;

            var create = new Guard
            {
                Date = request.Date ?? DateTime.Now,
                Hours = IsWeekend(request.Date) ? 24 : 16,
                Worker = worker!,
                WorkerId = worker!.Id
            };

            _appDbContext.Guards.Add(create);
            await _appDbContext.SaveChangesAsync(cancellationToken);

            return create.Id;
        }

        private static bool IsWeekend(DateTime? date)
        {
            return date!.Value.DayOfWeek == DayOfWeek.Saturday || date.Value.DayOfWeek == DayOfWeek.Sunday;
        }
    }
}
