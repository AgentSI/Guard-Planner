using Application.DTOs;
using Application.Interfaces;
using Application.Interfaces.Pagination;
using Domain.Entities;
using MediatR;
using System.Linq.Expressions;

namespace Application.Guards.Queries
{
    public class GuardListQuery : IRequest<PaginationResult<GuardDto>>
    {
        public GuardListQuery(PaginationParameter paginationParameter, Guid? workerId)
        {
            WorkerId = workerId;
            PaginationParameter = paginationParameter;
        }

        public Guid? WorkerId { get; set; }
        public PaginationParameter PaginationParameter { get; set; }
    }

    public class GuardListQueryHandler : IRequestHandler<GuardListQuery, PaginationResult<GuardDto>>
    {
        private readonly IPaginationService _paginationService;
        private readonly IAppDbContext _appDbContext;

        public GuardListQueryHandler(IPaginationService paginationService, IAppDbContext appDbContext)
        {
            _paginationService = paginationService;
            _appDbContext = appDbContext;
        }

        public async Task<PaginationResult<GuardDto>> Handle(GuardListQuery request, CancellationToken cancellationToken)
        {
            var query = request.PaginationParameter;
            IQueryable<Guard> guardsQuery = _appDbContext.Guards.AsQueryable();
            if (request.WorkerId != null) guardsQuery = _appDbContext.Guards.Where(o => o.WorkerId == request.WorkerId).AsQueryable();

            if (!string.IsNullOrEmpty(query.SortBy))
            {
                if (query.SortBy == nameof(GuardDto.Date))
                {
                    if (query.SortDescending.HasValue && query.SortDescending.Value) guardsQuery = guardsQuery.OrderByDescending(e => e.Date);
                    else guardsQuery = guardsQuery.OrderBy(e => e.Date);
                }
                else if (query.SortBy == nameof(GuardDto.NrOperations))
                {
                    if (query.SortDescending.HasValue && query.SortDescending.Value) guardsQuery = guardsQuery.OrderByDescending(e => e.Operations.Count);
                    else guardsQuery = guardsQuery.OrderBy(e => e.Operations.Count);
                }
                else if (query.SortBy == nameof(GuardDto.Hours))
                {
                    if (query.SortDescending.HasValue && query.SortDescending.Value) guardsQuery = guardsQuery.OrderByDescending(e => e.Hours);
                    else guardsQuery = guardsQuery.OrderBy(e => e.Hours);
                }
                else if (query.SortBy == nameof(GuardDto.WorkerName))
                {
                    if (query.SortDescending.HasValue && query.SortDescending.Value) guardsQuery = guardsQuery.OrderByDescending(e => e.Worker.Name);
                    else guardsQuery = guardsQuery.OrderBy(e => e.Worker.Name);
                }
            }
            else
            {
                guardsQuery = guardsQuery.OrderBy(e => e.Date);
            }

            return await _paginationService.PaginateAsync(guardsQuery, query, GuardMapping.GuardProjection);
        }
    }
}
