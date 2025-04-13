using Application.DTOs;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Users.Commands
{
    public class UserEditCommand(UserDto model) : IRequest<Unit>
    {
        public Guid Id { get; set; } = model.Id;
        public string? Email { get; set; } = model.Email;
        public string? Username { get; set; } = model.Username;
        public string? Password { get; set; } = model.Password;
        public string? Role { get; set; } = model.Role;
        public string? Phone { get; set; } = model.Phone;
        public DateTime? Birthday { get; set; } = model.Birthday;
    }

    public class UserEditCommandHandler(IAppDbContext appDbContext) : IRequestHandler<UserEditCommand, Unit>
    {
        private readonly IAppDbContext _appDbContext = appDbContext;

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
                userToEdit.UserRoleId = role!.Id;
                userToEdit.Phone = request.Phone;
                userToEdit.Birthday = request.Birthday;
            }

            await _appDbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
