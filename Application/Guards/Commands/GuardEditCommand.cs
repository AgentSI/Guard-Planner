using Application.DTOs;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Guards.Commands
{
    public class GuardEditCommand(GuardDto model) : IRequest<Unit>
    {
        public Guid Id { get; set; } = model.Id;
        public DateTime? Date { get; set; } = model.Date;
        public string? WorkerName { get; set; } = model.WorkerName;
    }

    public class GuardEditCommandHandler(IAppDbContext appDbContext) : IRequestHandler<GuardEditCommand, Unit>
    {
        private readonly IAppDbContext _appDbContext = appDbContext;

        public async Task<Unit> Handle(GuardEditCommand request, CancellationToken cancellationToken)
        {
            var worker = await _appDbContext.Workers.Where(w => w.Name == request.WorkerName)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);

            var toEdit = await _appDbContext.Guards
                .Where(p => p.Id == request.Id)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);

            if (toEdit != null)
            {
                toEdit.Date = request.Date ?? DateTime.Now;
                toEdit.Hours = IsWeekend(request.Date) ? 24 : 16;
                toEdit.Worker = worker!;
                toEdit.WorkerId = worker!.Id;
            }

            await _appDbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }

        private static bool IsWeekend(DateTime? date)
        {
            return date!.Value.DayOfWeek == DayOfWeek.Saturday || date.Value.DayOfWeek == DayOfWeek.Sunday;
        }
    }
}
