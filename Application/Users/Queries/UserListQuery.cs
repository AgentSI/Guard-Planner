using Application.DTOs;
using Application.Interfaces;
using Application.Interfaces.Pagination;
using Domain.Entities;
using MediatR;
using System.Linq.Expressions;

namespace Application.Users.Queries
{
    public class UserListQuery : IRequest<PaginationResult<UserDto>>
    {
        public UserListQuery(PaginationParameter paginationParameter)
        {
            PaginationParameter = paginationParameter;
        }
        public PaginationParameter PaginationParameter { get; set; }
    }

    public class UserListQueryHandler : IRequestHandler<UserListQuery, PaginationResult<UserDto>>
    {
        private readonly IPaginationService _paginationService;
        private readonly IAppDbContext _appDbContext;

        public UserListQueryHandler(IPaginationService paginationService, IAppDbContext appDbContext)
        {
            _paginationService = paginationService;
            _appDbContext = appDbContext;
        }

        public async Task<PaginationResult<UserDto>> Handle(UserListQuery request, CancellationToken cancellationToken)
        {
            var query = request.PaginationParameter;
            var users = _appDbContext.Users.AsQueryable();

            if (!string.IsNullOrEmpty(query.SortBy))
            {
                if (query.SortBy == nameof(UserDto.Role))
                {
                    if (query.SortDescending.HasValue && query.SortDescending.Value) users = users.OrderByDescending(u => u.UserRole.RoleName);
                    else users = users.OrderBy(u => u.UserRole.RoleName);
                }
                else
                {
                    var propertyInfo = typeof(User).GetProperty(query.SortBy);

                    if (propertyInfo != null)
                    {
                        var parameter = Expression.Parameter(typeof(User), "x");
                        var property = Expression.Property(parameter, query.SortBy);
                        var lambda = Expression.Lambda<Func<User, string>>(property, parameter);

                        if (query.SortDescending.HasValue && query.SortDescending.Value) users = users.OrderByDescending(lambda);
                        else users = users.OrderBy(lambda);
                    }
                }
            }
            else users = users.OrderBy(u => u.Username);

            return await _paginationService.PaginateAsync(users, query, UserAccountMapping.UserProjection);
        }
    }
}
