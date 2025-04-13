using Application.DTOs;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Vacations.Commands
{
    public class VacationEditCommand(VacationDto model) : IRequest<Unit>
    {
        public Guid Id { get; set; } = model.Id;
        public string? WorkerName { get; set; } = model.WorkerName;
        public DateTime? StartDate { get; set; } = model.StartDate;
        public DateTime? EndDate { get; set; } = model.EndDate;
        public string? Reason { get; set; } = model.Reason;
    }

    public class VacationEditCommandHandler(IAppDbContext appDbContext) : IRequestHandler<VacationEditCommand, Unit>
    {
        private readonly IAppDbContext _appDbContext = appDbContext;

        public async Task<Unit> Handle(VacationEditCommand request, CancellationToken cancellationToken)
        {
            var worker = await _appDbContext.Workers.Where(w => w.Name == request.WorkerName).FirstOrDefaultAsync();

            var toEdit = await _appDbContext.Vacations
                .Where(p => p.Id == request.Id)
                .FirstOrDefaultAsync();

            worker!.NoDaysVacation += toEdit!.NoDays;
            if (worker.NoDaysVacation < (request.EndDate!.Value - request.StartDate!.Value).Days + 1) return Unit.Value;
            if (toEdit != null)
            {
                toEdit.StartDate = request.StartDate ?? DateTime.Now;
                toEdit.EndDate = request.EndDate ?? DateTime.Now;
                toEdit.Reason = request.Reason;
                toEdit.Worker = worker;
                toEdit.WorkerId = worker.Id;
                toEdit.NoDays = (request.EndDate!.Value - request.StartDate!.Value).Days + 1;
            }

            worker.NoDaysVacation -= toEdit!.NoDays;
            await _appDbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
