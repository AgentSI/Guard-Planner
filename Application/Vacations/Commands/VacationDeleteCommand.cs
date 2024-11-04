using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Vacations.Commands
{
    public class VacationDeleteCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
    }

    public class VacationDeleteCommandHandler : IRequestHandler<VacationDeleteCommand, Unit>
    {
        private readonly IAppDbContext _appDbContext;

        public VacationDeleteCommandHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<Unit> Handle(VacationDeleteCommand request, CancellationToken cancellationToken)
        {
            var toDelete = await _appDbContext.Vacations.Where(e => e.Id == request.Id).FirstOrDefaultAsync();
            var worker = await _appDbContext.Workers.Where(w => w.Id == toDelete.WorkerId).FirstOrDefaultAsync();
            worker.NoDaysVacation += toDelete.NoDays; 

            _appDbContext.Vacations.Remove(toDelete);
            await _appDbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
