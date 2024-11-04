using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Operations.Commands
{
    public class OperationCreateCommand : IRequest<Guid>
    {
        public OperationCreateCommand(OperationDto create)
        {
            Type = create.Type;
            StartTime = create.StartTime;
            EndTime = create.EndTime;
            GuardId = create.GuardId;
        }

        public string? Type { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public Guid GuardId { get; set; }
    }

    public class OperationCreateCommandHandler : IRequestHandler<OperationCreateCommand, Guid>
    {
        private readonly IAppDbContext _appDbContext;

        public OperationCreateCommandHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<Guid> Handle(OperationCreateCommand request, CancellationToken cancellationToken)
        {
            var create = new Operation
            {
                Type = request.Type,
                StartTime = request.StartTime ?? TimeSpan.Zero,
                EndTime = request.EndTime ?? TimeSpan.Zero,
                GuardId = request.GuardId
            };

            _appDbContext.Operations.Add(create);
            await _appDbContext.SaveChangesAsync(cancellationToken);

            return create.Id;
        }
    }
}
