using Application.DTOs;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Operations.Commands
{
    public class OperationEditCommand : IRequest<Unit>
    {
        public OperationEditCommand(OperationDto model)
        {
            Id = model.Id;
            Type = model.Type;
            StartTime = model.StartTime;
            EndTime = model.EndTime;
            GuardId = model.GuardId;
        }

        public Guid Id { get; set; }
        public string? Type { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public Guid GuardId { get; set; }
    }

    public class OperationEditCommandHandler : IRequestHandler<OperationEditCommand, Unit>
    {
        private readonly IAppDbContext _appDbContext;

        public OperationEditCommandHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<Unit> Handle(OperationEditCommand request, CancellationToken cancellationToken)
        {
            var toEdit = await _appDbContext.Operations.Where(p => p.Id == request.Id).FirstOrDefaultAsync();

            if (toEdit != null)
            {
                toEdit.Type = request.Type;
                toEdit.StartTime = request.StartTime ?? TimeSpan.Zero;
                toEdit.EndTime = request.EndTime ?? TimeSpan.Zero;
                toEdit.GuardId = request.GuardId;
            }

            await _appDbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
