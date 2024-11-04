using Application.DTOs;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Application.Workers.Queries
{
    public class GetPercentListQuery : IRequest<List<PercentDto>>
    {
        public GetPercentListQuery()
        {
        }
    }

    public class GetPercentListQueryHandler : IRequestHandler<GetPercentListQuery, List<PercentDto>>
    {
        private readonly IAppDbContext _appDbContext;

        public GetPercentListQueryHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<List<PercentDto>> Handle(GetPercentListQuery request, CancellationToken cancellationToken)
        {
            var percents = await _appDbContext.Percents
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            return percents.Select(percent => WorkerMapping.PercentProjection(percent)).ToList();
        }
    }
}
