using Application.DTOs;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Operations.Commands
{
    public class OperationEditCommand(OperationDto model) : IRequest<Unit>
    {
        public Guid Id { get; set; } = model.Id;
        public string? Type { get; set; } = model.Type;
        public TimeSpan? StartTime { get; set; } = model.StartTime;
        public TimeSpan? EndTime { get; set; } = model.EndTime;
        public Guid GuardId { get; set; } = model.GuardId;
    }

    public class OperationEditCommandHandler(IAppDbContext appDbContext) : IRequestHandler<OperationEditCommand, Unit>
    {
        private readonly IAppDbContext _appDbContext = appDbContext;

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
