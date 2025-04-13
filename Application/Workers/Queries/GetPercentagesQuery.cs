using Application.Interfaces;
using MediatR;

namespace Application.Workers.Queries
{
    public class GetPercentagesQuery : IRequest<List<double>>
    {
    }

    public class GetPercentagesQueryHandler(IAppDbContext appDbContext) : IRequestHandler<GetPercentagesQuery, List<double>>
    {
        private readonly IAppDbContext _appDbContext = appDbContext;

        public Task<List<double>> Handle(GetPercentagesQuery request, CancellationToken cancellationToken)
        {
            var percentages = _appDbContext.Percents.Select(p => p.Value).ToList();
            return Task.FromResult(percentages);
        }
    }
}
