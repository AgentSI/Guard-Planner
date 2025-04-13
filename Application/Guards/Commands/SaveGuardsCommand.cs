using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Guards.Commands
{
    public class SaveGuardsCommand(List<GuardDto> guards) : IRequest<Unit>
    {
        public List<GuardDto> Guards { get; set; } = guards;
    }

    public class SaveGuardsCommandHandler(IAppDbContext appDbContext) : IRequestHandler<SaveGuardsCommand, Unit>
    {
        private readonly IAppDbContext _appDbContext = appDbContext;

        public async Task<Unit> Handle(SaveGuardsCommand request, CancellationToken cancellationToken)
        {
            var guards = request.Guards.Select(dto => new Guard
            {
                WorkerId = dto.WorkerId,
                Date = dto.Date!.Value,
                Hours = dto.Hours,
            }).ToList();

            await _appDbContext.Guards.AddRangeAsync(guards, cancellationToken);
            await _appDbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
