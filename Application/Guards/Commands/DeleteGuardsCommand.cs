using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Guards.Commands
{
    public class DeleteGuardsCommand : IRequest<Unit>
    {
        public DeleteGuardsCommand(DateTime? date)
        {
            Date = date;
        }

        public DateTime? Date { get; set; }
    }

    public class DeleteGuardsCommandHandler : IRequestHandler<DeleteGuardsCommand, Unit>
    {
        private readonly IAppDbContext _appDbContext;

        public DeleteGuardsCommandHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

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
