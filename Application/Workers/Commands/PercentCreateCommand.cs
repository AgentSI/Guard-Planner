using Application.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Workers.Commands
{
    public class PercentCreateCommand : IRequest<Guid>
    {
        public PercentCreateCommand(double percent)
        {
            Percent = percent;
        }

        public double Percent { get; set; }
    }

    public class PercentCreateCommandHandler : IRequestHandler<PercentCreateCommand, Guid>
    {
        private readonly IAppDbContext _appDbContext;

        public PercentCreateCommandHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

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
