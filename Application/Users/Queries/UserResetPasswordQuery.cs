using Application.Interfaces;
using Domain.Entities;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Users.Queries
{
    public class UserResetPasswordQuery(string email, string password) : IRequest<Unit>
    {
        public string Email { get; set; } = email;
        public string Password { get; set; } = password;
    }

    public class UserResetPasswordCommandHandler(IAppDbContext appDbContext) : IRequestHandler<UserResetPasswordQuery, Unit>
    {
        private readonly IAppDbContext _appDbContext = appDbContext;

        public async Task<Unit> Handle(UserResetPasswordQuery request, CancellationToken cancellationToken)
        {
            var userToEdit = await _appDbContext.Users.Where(p => p.Email == request.Email).FirstOrDefaultAsync();
            if (userToEdit != null) userToEdit.PasswordHash = Crypto.HashPassword(AuthorizationVariables.Salt + request.Password);
            await _appDbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
