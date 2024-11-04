using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Guards.Commands
{
    public class SaveGuardsCommand : IRequest<Unit>
    {
        public SaveGuardsCommand(List<GuardDto> guards)
        {
            Guards = guards;
        }

        public List<GuardDto> Guards { get; set; }
    }

    public class SaveGuardsCommandHandler : IRequestHandler<SaveGuardsCommand, Unit>
    {
        private readonly IAppDbContext _appDbContext;

        public SaveGuardsCommandHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<Unit> Handle(SaveGuardsCommand request, CancellationToken cancellationToken)
        {
            var guards = request.Guards.Select(dto => new Guard
            {
                WorkerId = dto.WorkerId,
                Date = dto.Date.Value,
                Hours = dto.Hours,
            }).ToList();

            await _appDbContext.Guards.AddRangeAsync(guards, cancellationToken);
            await _appDbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
