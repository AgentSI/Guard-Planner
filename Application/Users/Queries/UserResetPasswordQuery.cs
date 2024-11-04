using Application.Interfaces;
using Domain.Entities;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Users.Queries
{
    public class UserResetPasswordQuery : IRequest<Unit>
    {
        public UserResetPasswordQuery(string email, string password)
        {
            Email = email;
            Password = password;
        }

        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class UserResetPasswordCommandHandler : IRequestHandler<UserResetPasswordQuery, Unit>
    {
        private readonly IAppDbContext _appDbContext;

        public UserResetPasswordCommandHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<Unit> Handle(UserResetPasswordQuery request, CancellationToken cancellationToken)
        {
            var userToEdit = await _appDbContext.Users.Where(p => p.Email == request.Email).FirstOrDefaultAsync();
            if (userToEdit != null) userToEdit.PasswordHash = Crypto.HashPassword(AuthorizationVariables.Salt + request.Password);
            await _appDbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
