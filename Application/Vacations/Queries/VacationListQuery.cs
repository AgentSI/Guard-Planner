using Application.DTOs;
using Application.Interfaces;
using Application.Interfaces.Pagination;
using Domain.Entities;
using MediatR;

namespace Application.Vacations.Queries
{
    public class VacationListQuery(PaginationParameter paginationParameter, Guid? workerId) : IRequest<PaginationResult<VacationDto>>
    {
        public Guid? WorkerId { get; set; } = workerId;
        public PaginationParameter PaginationParameter { get; set; } = paginationParameter;
    }

    public class VacationListQueryHandler(IPaginationService paginationService, IAppDbContext appDbContext) : IRequestHandler<VacationListQuery, PaginationResult<VacationDto>>
    {
        private readonly IPaginationService _paginationService = paginationService;
        private readonly IAppDbContext _appDbContext = appDbContext;

        public async Task<PaginationResult<VacationDto>> Handle(VacationListQuery request, CancellationToken cancellationToken)
        {
            var query = request.PaginationParameter;
            IQueryable<Vacation> vacations = _appDbContext.Vacations.AsQueryable();
            if (request.WorkerId != null) vacations = _appDbContext.Vacations.Where(o => o.WorkerId == request.WorkerId).AsQueryable();

            if (!string.IsNullOrEmpty(query.SortBy))
            {
                if (query.SortBy == nameof(VacationDto.StartDate))
                {
                    if (query.SortDescending.HasValue && query.SortDescending.Value) vacations = vacations.OrderByDescending(e => e.StartDate);
                    else vacations = vacations.OrderBy(e => e.StartDate);
                }
                else if (query.SortBy == nameof(VacationDto.EndDate))
                {
                    if (query.SortDescending.HasValue && query.SortDescending.Value) vacations = vacations.OrderByDescending(e => e.EndDate);
                    else vacations = vacations.OrderBy(e => e.EndDate);
                }
                else if (query.SortBy == nameof(VacationDto.NoDays))
                {
                    if (query.SortDescending.HasValue && query.SortDescending.Value) vacations = vacations.OrderByDescending(e => e.NoDays);
                    else vacations = vacations.OrderBy(e => e.NoDays);
                }
                else if (query.SortBy == nameof(VacationDto.WorkerName))
                {
                    if (query.SortDescending.HasValue && query.SortDescending.Value) vacations = vacations.OrderByDescending(e => e.Worker!.Name);
                    else vacations = vacations.OrderBy(e => e.Worker!.Name);
                }
            }
            else
            {
                vacations = vacations.OrderBy(e => e.StartDate);
            }

            return await _paginationService.PaginateAsync(vacations, query, VacationMapping.VacationProjection);
        }
    }
}
