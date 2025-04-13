using Application.DTOs;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Workers.Commands
{
    public class WorkerEditCommand(WorkerDto model) : IRequest<Unit>
    {
        public Guid Id { get; set; } = model.Id;
        public string? Name { get; set; } = model.Name;
        public string? FirstName { get; set; } = model.FirstName;
        public string? Specialization { get; set; } = model.Specialization;
        public string? Email { get; set; } = model.Email;
        public bool Available { get; set; } = model.Available;
        public bool IsWorkDay { get; set; } = model.IsWorkDay;
        public bool IsGuard { get; set; } = model.IsGuard;
        public double Percent { get; set; } = model.Percent;
        public int NoDaysVacation { get; set; } = model.NoDaysVacation;
    }

    public class WorkerEditCommandHandler : IRequestHandler<WorkerEditCommand, Unit>
    {
        private readonly IAppDbContext _appDbContext;

        public WorkerEditCommandHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<Unit> Handle(WorkerEditCommand request, CancellationToken cancellationToken)
        {
            var toEdit = await _appDbContext.Workers
                .Where(p => p.Id == request.Id)
                .FirstOrDefaultAsync();

            if (toEdit != null)
            {
                toEdit.Name = request.Name;
                toEdit.FirstName = request.FirstName;
                toEdit.Specialization = request.Specialization;
                toEdit.Email = request.Email;
                toEdit.Available = request.Available;
                toEdit.IsWorkDay = request.IsWorkDay;
                toEdit.IsGuard = request.IsGuard;
                toEdit.Percent = request.Percent;
                toEdit.NoDaysVacation = request.NoDaysVacation;
            }

            await _appDbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
