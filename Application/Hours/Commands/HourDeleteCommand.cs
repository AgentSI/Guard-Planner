using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Hours.Commands
{
    public class HourDeleteCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
    }

    public class HourDeleteCommandHandler(IAppDbContext appDbContext) : IRequestHandler<HourDeleteCommand, Unit>
    {
        private readonly IAppDbContext _appDbContext = appDbContext;

        public async Task<Unit> Handle(HourDeleteCommand request, CancellationToken cancellationToken)
        {
            var toDelete = await _appDbContext.Hours.Where(e => e.Id == request.Id).FirstOrDefaultAsync();

            _appDbContext.Hours.Remove(toDelete!);
            await _appDbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
