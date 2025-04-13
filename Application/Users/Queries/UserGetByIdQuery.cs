using Application.DTOs;
using Application.Interfaces;
using MediatR;

namespace Application.Users.Queries
{
    public class UserGetByIdQuery(Guid id) : IRequest<UserDto>
    {
        public Guid Id { get; set; } = id;
    }

    public class UserGetByIdQueryHandler(IAppDbContext appDbContext) : IRequestHandler<UserGetByIdQuery, UserDto>
    {
        private readonly IAppDbContext _appDbContext = appDbContext;

        public Task<UserDto> Handle(UserGetByIdQuery request, CancellationToken cancellationToken)
        {
            var user = _appDbContext.Users.Where(p => p.Id == request.Id).Select(UserAccountMapping.UserProjection).FirstOrDefault();
            if (user != null) return Task.FromResult(user);
            else return Task.FromResult(new UserDto { });
        }
    }
}
