using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Operations.Commands
{
    public class OperationCreateCommand(OperationDto create) : IRequest<Guid>
    {
        public string? Type { get; set; } = create.Type;
        public TimeSpan? StartTime { get; set; } = create.StartTime;
        public TimeSpan? EndTime { get; set; } = create.EndTime;
        public Guid GuardId { get; set; } = create.GuardId;
    }

    public class OperationCreateCommandHandler(IAppDbContext appDbContext) : IRequestHandler<OperationCreateCommand, Guid>
    {
        private readonly IAppDbContext _appDbContext = appDbContext;

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
