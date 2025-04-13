using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Workers.Commands
{
    public class WorkerCreateCommand(WorkerDto create) : IRequest<Guid>
    {
        public string? Name { get; set; } = create.Name;
        public string? FirstName { get; set; } = create.FirstName;
        public string? Specialization { get; set; } = create.Specialization;
        public string? Email { get; set; } = create.Email;
        public bool Available { get; set; } = create.Available;
        public bool IsWorkDay { get; set; } = create.IsWorkDay;
        public bool IsGuard { get; set; } = create.IsGuard;
        public double Percent { get; set; } = create.Percent;
    }

    public class WorkerCreateCommandHandler : IRequestHandler<WorkerCreateCommand, Guid>
    {
        private readonly IAppDbContext _appDbContext;

        public WorkerCreateCommandHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<Guid> Handle(WorkerCreateCommand request, CancellationToken cancellationToken)
        {
            var create = new Worker
            {
                Name = request.Name,
                FirstName = request.FirstName,
                Specialization = request.Specialization,
                Email = request.Email,
                Available = request.Available,
                IsWorkDay = request.IsWorkDay,
                IsGuard = request.IsGuard,
                Percent = request.Percent,
            };

            _appDbContext.Workers.Add(create);
            await _appDbContext.SaveChangesAsync(cancellationToken);

            return create.Id;
        }
    }
}
