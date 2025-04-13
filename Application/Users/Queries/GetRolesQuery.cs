using Application.Interfaces;
using MediatR;

namespace Application.Users.Queries
{
    public class GetRolesQuery : IRequest<List<string>> { }

    public class GetRolesQueryHandler(IAppDbContext appDbContext) : IRequestHandler<GetRolesQuery, List<string>>
    {
        private readonly IAppDbContext _appDbContext = appDbContext;

        public Task<List<string>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
        {
            var roles = _appDbContext.UserRoles.Select(role => role.RoleName).ToList();
            return Task.FromResult<List<string>>(roles!);
        }
    }
}
