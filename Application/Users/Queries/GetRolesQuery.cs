using Application.Interfaces;
using MediatR;

namespace Application.Users.Queries
{
    public class GetRolesQuery : IRequest<List<string>>
    {
    }

    public class GetRolesQueryHandler : IRequestHandler<GetRolesQuery, List<string>>
    {
        private readonly IAppDbContext _appDbContext;
        public GetRolesQueryHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<List<string>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
        {
            var roles = _appDbContext.UserRoles.Select(role => role.RoleName).ToList();
            return roles;
        }
    }
}
