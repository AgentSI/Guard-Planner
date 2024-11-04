using Application.DTOs;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Users.Commands
{
    public class UserEditCommand : IRequest<Unit>
    {
        public UserEditCommand(UserDto model)
        {
            Id = model.Id;
            Email = model.Email;
            Username = model.Username;
            Password = model.Password;
            Role = model.Role;
        }

        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string? Password { get; set; }
        public string Role { get; set; }
    }

    public class UserEditCommandHandler : IRequestHandler<UserEditCommand, Unit>
    {
        private readonly IAppDbContext _appDbContext;

        public UserEditCommandHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<Unit> Handle(UserEditCommand request, CancellationToken cancellationToken)
        {
            var userToEdit = await _appDbContext.Users
                .Include(u => u.UserRole)
                .Where(p => p.Id == request.Id)
                .FirstOrDefaultAsync();

            var role = await _appDbContext.UserRoles.Where(r => r.RoleName == request.Role).FirstOrDefaultAsync();

            if (userToEdit != null)
            {
                userToEdit.Email = request.Email;
                userToEdit.Username = request.Username;
                userToEdit.UserRole = role;
                userToEdit.UserRoleId = role.Id;
            }

            await _appDbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
