using System.Linq.Expressions;

namespace Application.Interfaces.Pagination
{
    public interface IPaginationService
    {
        Task<PaginationResult<DestinationT>> PaginateAsync<TSource, DestinationT>(IQueryable<TSource> source, PaginationParameter request, Expression<Func<TSource, DestinationT>> MappingRule);

        Task<PaginationResult<TDestination>> PaginateAsyncEnumerable<TSource, TDestination>(
            IAsyncEnumerable<TSource> source,
            PaginationParameter request,
            Func<TSource, TDestination> mappingRule);
    }
}
