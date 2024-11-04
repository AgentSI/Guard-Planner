using Application.DTOs;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Workers.Commands
{
    public class WorkerEditCommand : IRequest<Unit>
    {
        public WorkerEditCommand(WorkerDto model)
        {
            Id = model.Id;
            Name = model.Name;
            FirstName = model.FirstName;
            Specialization = model.Specialization;
            Email = model.Email;
            Available = model.Available;
            IsGuard = model.IsGuard;
            Percent = model.Percent;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? FirstName { get; set; }
        public string? Specialization { get; set; }
        public string? Email { get; set; }
        public bool Available { get; set; }
        public bool IsGuard { get; set; }
        public double Percent { get; set; }
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
                toEdit.IsGuard = request.IsGuard;
                toEdit.Percent = request.Percent;
            }

            await _appDbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
