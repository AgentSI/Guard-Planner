using Application.DTOs;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Guards.Commands
{
    public class GuardEditCommand : IRequest<Unit>
    {
        public GuardEditCommand(GuardDto model)
        {
            Id = model.Id;
            Date = model.Date;
            WorkerName = model.WorkerName;
        }

        public Guid Id { get; set; }
        public DateTime? Date { get; set; }
        public string WorkerName { get; set; }
    }

    public class GuardEditCommandHandler : IRequestHandler<GuardEditCommand, Unit>
    {
        private readonly IAppDbContext _appDbContext;

        public GuardEditCommandHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<Unit> Handle(GuardEditCommand request, CancellationToken cancellationToken)
        {
            var worker = await _appDbContext.Workers.Where(w => w.Name == request.WorkerName).FirstOrDefaultAsync();

            var toEdit = await _appDbContext.Guards
                .Where(p => p.Id == request.Id)
                .FirstOrDefaultAsync();

            if (toEdit != null)
            {
                toEdit.Date = request.Date ?? DateTime.Now;
                toEdit.Hours = IsWeekend(request.Date) ? 24 : 16;
                toEdit.Worker = worker;
                toEdit.WorkerId = worker.Id;
            }

            await _appDbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }

        private bool IsWeekend(DateTime? date)
        {
            return date.Value.DayOfWeek == DayOfWeek.Saturday || date.Value.DayOfWeek == DayOfWeek.Sunday;
        }
    }
}
