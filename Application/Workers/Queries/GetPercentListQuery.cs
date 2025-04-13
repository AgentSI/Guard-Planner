using Application.DTOs;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Workers.Queries
{
    public class GetPercentListQuery : IRequest<List<PercentDto>>
    {
        public GetPercentListQuery() { }
    }

    public class GetPercentListQueryHandler(IAppDbContext appDbContext) : IRequestHandler<GetPercentListQuery, List<PercentDto>>
    {
        private readonly IAppDbContext _appDbContext = appDbContext;

        public async Task<List<PercentDto>> Handle(GetPercentListQuery request, CancellationToken cancellationToken)
        {
            var percents = await _appDbContext.Percents
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            return percents.Select(percent => WorkerMapping.PercentProjection(percent)).ToList();
        }
    }
}
