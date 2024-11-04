using Application.DTOs;
using Application.Interfaces;
using Application.Interfaces.Pagination;
using Domain.Entities;
using MediatR;
using System.Linq.Expressions;

namespace Application.Operations.Queries
{
    public class OperationListQuery : IRequest<PaginationResult<OperationDto>>
    {
        public OperationListQuery(PaginationParameter paginationParameter, Guid? guardId)
        {
            GuardId = guardId;
            PaginationParameter = paginationParameter;
        }

        public Guid? GuardId { get; set; }
        public PaginationParameter PaginationParameter { get; set; }
    }

    public class OperationListQueryHandler : IRequestHandler<OperationListQuery, PaginationResult<OperationDto>>
    {
        private readonly IPaginationService _paginationService;
        private readonly IAppDbContext _appDbContext;

        public OperationListQueryHandler(IPaginationService paginationService, IAppDbContext appDbContext)
        {
            _paginationService = paginationService;
            _appDbContext = appDbContext;
        }

        public async Task<PaginationResult<OperationDto>> Handle(OperationListQuery request, CancellationToken cancellationToken)
        {
            var query = request.PaginationParameter;
            IQueryable<Operation> operationsQuery = _appDbContext.Operations.AsQueryable();
            if (request.GuardId != null) operationsQuery = _appDbContext.Operations.Where(o => o.GuardId == request.GuardId).AsQueryable();

            if (!string.IsNullOrEmpty(query.SortBy))
            {
                if (query.SortBy == nameof(OperationDto.StartTime))
                {
                    if (query.SortDescending.HasValue && query.SortDescending.Value) operationsQuery = operationsQuery.OrderByDescending(u => u.StartTime);
                    else operationsQuery = operationsQuery.OrderBy(u => u.StartTime);
                }
                else if (query.SortBy == nameof(OperationDto.EndTime))
                {
                    if (query.SortDescending.HasValue && query.SortDescending.Value) operationsQuery = operationsQuery.OrderByDescending(u => u.EndTime);
                    else operationsQuery = operationsQuery.OrderBy(u => u.EndTime);
                }
                else if (query.SortBy == nameof(OperationDto.Date))
                {
                    if (query.SortDescending.HasValue && query.SortDescending.Value) operationsQuery = operationsQuery.OrderByDescending(u => u.Guard.Date);
                    else operationsQuery = operationsQuery.OrderBy(u => u.Guard.Date);
                }
                else
                {
                    var propertyInfo = typeof(Operation).GetProperty(query.SortBy);

                    if (propertyInfo != null)
                    {
                        var parameter = Expression.Parameter(typeof(Operation), "x");
                        var property = Expression.Property(parameter, query.SortBy);
                        var lambda = Expression.Lambda<Func<Operation, string>>(property, parameter);

                        if (query.SortDescending.HasValue && query.SortDescending.Value) operationsQuery = operationsQuery.OrderByDescending(lambda);
                        else operationsQuery = operationsQuery.OrderBy(lambda);
                    }
                }
            }
            else operationsQuery = operationsQuery.OrderBy(u => u.StartTime);

            return await _paginationService.PaginateAsync(operationsQuery, query, OperationMapping.OperationProjection);
        }
    }
}
