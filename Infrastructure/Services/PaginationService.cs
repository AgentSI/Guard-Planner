using Application.Interfaces.Pagination;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class PaginationService : IPaginationService
    {
        public async Task<PaginationResult<DestinationT>> PaginateAsync<TSource, DestinationT>(IQueryable<TSource> source, PaginationParameter request, Expression<Func<TSource, DestinationT>> MappingRule)
        {
            var response = new PaginationResult<DestinationT>();

            response.TotalItems = source.Count();
            var count = source.Count();
            var items = await source
                                .Skip((request.PageNumber - 1) * request.PageSize)
                                .Take(request.PageSize)
                                .Select(MappingRule)
                                .ToListAsync();

            response.Items = items;
            return response;
        }

        public Task<PaginationResult<TDestination>> PaginateAsyncEnumerable<TSource, TDestination>(IAsyncEnumerable<TSource> source, PaginationParameter request, Func<TSource, TDestination> mappingRule)
        {
            throw new NotImplementedException();
        }
    }
}
