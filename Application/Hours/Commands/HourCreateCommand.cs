using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Hours.Commands
{
    public class HourCreateCommand(HourDto hour) : IRequest<Guid>
    {
        public int Value { get; set; } = hour.Value;
        public string? Label { get; set; } = hour.Label;
    }

    public class HourCreateCommandHandler(IAppDbContext appDbContext) : IRequestHandler<HourCreateCommand, Guid>
    {
        private readonly IAppDbContext _appDbContext = appDbContext;

        public async Task<Guid> Handle(HourCreateCommand request, CancellationToken cancellationToken)
        {
            var existing = _appDbContext.Hours.FirstOrDefault(u => u.Value == request.Value || u.Label == request.Label);
            if (existing != null) return Guid.Empty;
            
            var create = new Hour
            {
                Value = request.Value,
                Label = request.Label
            };

            _appDbContext.Hours.Add(create);
            await _appDbContext.SaveChangesAsync(cancellationToken);

            return create.Id;
        }
    }
}
