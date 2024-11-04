using Application.Interfaces;
using MediatR;

namespace Application.Workers.Queries
{
    public class GetPercentagesQuery : IRequest<List<double>>
    {
    }

    public class GetPercentagesQueryHandler : IRequestHandler<GetPercentagesQuery, List<double>>
    {
        private readonly IAppDbContext _appDbContext;
        public GetPercentagesQueryHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<List<double>> Handle(GetPercentagesQuery request, CancellationToken cancellationToken)
        {
            var percentages = _appDbContext.Percents.Select(p => p.Value).ToList();
            return percentages;
        }
    }
}
