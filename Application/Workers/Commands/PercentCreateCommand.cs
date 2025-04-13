using Application.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Workers.Commands
{
    public class PercentCreateCommand(double percent) : IRequest<Guid>
    {
        public double Percent { get; set; } = percent;
    }

    public class PercentCreateCommandHandler(IAppDbContext appDbContext) : IRequestHandler<PercentCreateCommand, Guid>
    {
        private readonly IAppDbContext _appDbContext = appDbContext;

        public async Task<Guid> Handle(PercentCreateCommand request, CancellationToken cancellationToken)
        {
            var existing = _appDbContext.Percents.FirstOrDefault(u => u.Value == request.Percent);
            if (existing != null) return Guid.Empty;
            
            var create = new Percent
            {
                Value = request.Percent,
            };

            _appDbContext.Percents.Add(create);
            await _appDbContext.SaveChangesAsync(cancellationToken);

            return create.Id;
        }
    }
}
