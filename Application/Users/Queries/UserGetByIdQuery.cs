using Application.DTOs;
using Application.Interfaces;
using MediatR;

namespace Application.Users.Queries
{
    public class UserGetByIdQuery : IRequest<UserDto>
    {
        public UserGetByIdQuery(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }

    public class UserGetByIdQueryHandler : IRequestHandler<UserGetByIdQuery, UserDto>
    {
        private readonly IAppDbContext _appDbContext;

        public UserGetByIdQueryHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<UserDto> Handle(UserGetByIdQuery request, CancellationToken cancellationToken)
        {
            var user = _appDbContext.Users.Where(p => p.Id == request.Id).Select(UserAccountMapping.UserProjection).FirstOrDefault();
            if (user != null) return user;
            else return null;
        }
    }
}
