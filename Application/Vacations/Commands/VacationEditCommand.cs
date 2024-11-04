using Application.DTOs;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Vacations.Commands
{
    public class VacationEditCommand : IRequest<Unit>
    {
        public VacationEditCommand(VacationDto model)
        {
            Id = model.Id;
            WorkerName = model.WorkerName;
            StartDate = model.StartDate;
            EndDate = model.EndDate;
            Reason = model.Reason;
        }

        public Guid Id { get; set; }
        public string WorkerName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Reason { get; set; }
    }

    public class VacationEditCommandHandler : IRequestHandler<VacationEditCommand, Unit>
    {
        private readonly IAppDbContext _appDbContext;

        public VacationEditCommandHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<Unit> Handle(VacationEditCommand request, CancellationToken cancellationToken)
        {
            var worker = await _appDbContext.Workers.Where(w => w.Name == request.WorkerName).FirstOrDefaultAsync();

            var toEdit = await _appDbContext.Vacations
                .Where(p => p.Id == request.Id)
                .FirstOrDefaultAsync();

            worker.NoDaysVacation += toEdit.NoDays;
            if (worker.NoDaysVacation < (request.EndDate.Value - request.StartDate.Value).Days + 1) return Unit.Value;
            if (toEdit != null)
            {
                toEdit.StartDate = request.StartDate ?? DateTime.Now;
                toEdit.EndDate = request.EndDate ?? DateTime.Now;
                toEdit.Reason = request.Reason;
                toEdit.Worker = worker;
                toEdit.WorkerId = worker.Id;
                toEdit.NoDays = (request.EndDate.Value - request.StartDate.Value).Days + 1;
            }

            worker.NoDaysVacation -= toEdit.NoDays;
            await _appDbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
