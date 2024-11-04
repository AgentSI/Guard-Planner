using Application.DTOs;
using Application.Interfaces;
using Application.Interfaces.Pagination;
using Domain.Entities;
using MediatR;
using System.Linq.Expressions;

namespace Application.Workers.Queries
{
    public class WorkerListQuery : IRequest<PaginationResult<WorkerDto>>
    {
        public WorkerListQuery(PaginationParameter paginationParameter)
        {
            PaginationParameter = paginationParameter;
        }
        public PaginationParameter PaginationParameter { get; set; }
    }

    public class WorkerListQueryHandler : IRequestHandler<WorkerListQuery, PaginationResult<WorkerDto>>
    {
        private readonly IPaginationService _paginationService;
        private readonly IAppDbContext _appDbContext;

        public WorkerListQueryHandler(IPaginationService paginationService, IAppDbContext appDbContext)
        {
            _paginationService = paginationService;
            _appDbContext = appDbContext;
        }

        public async Task<PaginationResult<WorkerDto>> Handle(WorkerListQuery request, CancellationToken cancellationToken)
        {
            var query = request.PaginationParameter;
            var workers = _appDbContext.Workers.AsQueryable();

            if (!string.IsNullOrEmpty(query.SortBy))
            {
                if (query.SortBy == nameof(WorkerDto.NoDaysVacation))
                {
                    if (query.SortDescending.HasValue && query.SortDescending.Value) workers = workers.OrderByDescending(u => u.NoDaysVacation);
                    else workers = workers.OrderBy(u => u.NoDaysVacation);
                }
                else if (query.SortBy == nameof(WorkerDto.Percent))
                {
                    if (query.SortDescending.HasValue && query.SortDescending.Value) workers = workers.OrderByDescending(u => u.Percent);
                    else workers = workers.OrderBy(u => u.Percent);
                }
                else
                {
                    var propertyInfo = typeof(Worker).GetProperty(query.SortBy);

                    if (propertyInfo != null)
                    {
                        var parameter = Expression.Parameter(typeof(Worker), "x");
                        var property = Expression.Property(parameter, query.SortBy);
                        var lambda = Expression.Lambda<Func<Worker, string>>(property, parameter);

                        if (query.SortDescending.HasValue && query.SortDescending.Value) workers = workers.OrderByDescending(lambda);
                        else workers = workers.OrderBy(lambda);
                    }
                }
            }
            else workers = workers.OrderBy(u => u.Name);

            return await _paginationService.PaginateAsync(workers, query, WorkerMapping.WorkerProjection);
        }
    }
}
