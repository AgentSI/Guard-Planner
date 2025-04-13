using System.Linq.Expressions;

namespace Application.Interfaces.Pagination
{
    public interface IPaginationService
    {
        Task<PaginationResult<DestinationT>> PaginateAsync<TSource, DestinationT>(IQueryable<TSource> source, PaginationParameter request, Expression<Func<TSource, DestinationT>> MappingRule);
    }
}
