using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Users.Commands
{
    public class UserCreateCommand : IRequest<UserCreateResultDto>
    {
        public UserCreateCommand(UserDto createUser)
        {
            Email = createUser.Email;
            Username = createUser.Username;
            Password = createUser.Password;
            UserRole = createUser.Role;
        }

        public string? Email { get; set; }
        public string? Username { get; set; }
        public string? LastName { get; set; }
        public string? Password { get; set; }
        public string? UserRole { get; set; }
    }

    public class UserCreateCommandHandler : IRequestHandler<UserCreateCommand, UserCreateResultDto>
    {
        private readonly IAppDbContext _appDbContext;

        public UserCreateCommandHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<UserCreateResultDto> Handle(UserCreateCommand request, CancellationToken cancellationToken)
        {
            var existingUser = _appDbContext.Users.FirstOrDefault(u => u.Email == request.Email);
            if (existingUser != null)
            {
                return new UserCreateResultDto
                {
                    Success = false,
                    Message = $"Utilizator cu e-mailul {request.Email} există."
                };
            }
            var role = await _appDbContext.UserRoles.Where(r => r.RoleName == request.UserRole).FirstOrDefaultAsync();
            if (role == null) role = await _appDbContext.UserRoles.Where(r => r.RoleName == "Member").FirstOrDefaultAsync();
            var createUser = new User
            {
                Email = request.Email,
                Username = request.Username,
                UserRole = role,
                UserRoleId = role.Id,
                SecurityCode = "",
                CreatedAt = DateTime.UtcNow,
                PasswordHash = Crypto.HashPassword(AuthorizationVariables.Salt + request.Password)
            };

            _appDbContext.Users.Add(createUser);
            await _appDbContext.SaveChangesAsync(cancellationToken);

            return new UserCreateResultDto
            {
                Success = true,
                Message = "Utilizator creat cu succes."
            };
        }
    }
}
