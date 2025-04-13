using Application.Interfaces;
using MediatR;

namespace Application.Guards.Commands
{
    public class DeleteGuardsCommand(DateTime? date) : IRequest<Unit>
    {
        public DateTime? Date { get; set; } = date;
    }

    public class DeleteGuardsCommandHandler(IAppDbContext appDbContext) : IRequestHandler<DeleteGuardsCommand, Unit>
    {
        private readonly IAppDbContext _appDbContext = appDbContext;

        public async Task<Unit> Handle(DeleteGuardsCommand request, CancellationToken cancellationToken)
        {
            if (request.Date.HasValue)
            {
                var year = request.Date.Value.Year;
                var month = request.Date.Value.Month;

                var guardsToDelete = _appDbContext.Guards.Where(g => g.Date.Year == year && g.Date.Month == month);

                _appDbContext.Guards.RemoveRange(guardsToDelete);

                await _appDbContext.SaveChangesAsync(cancellationToken);
            }

            return Unit.Value;
        }
    }
}
