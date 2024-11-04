using Application.DTOs;
using Domain.Entities;
using System.Linq.Expressions;

namespace Application.Users
{
    public static class UserAccountMapping
    {
        public static Expression<Func<User, UserDto>> UserProjection
        {
            get
            {
                return u => new UserDto
                {
                    Id = u.Id,
                    Username = u.Username,
                    Email = u.Email,
                    Role = u.UserRole.RoleName,
                    UserRoleId = u.UserRole.Id,
                    CreatedAt = u.CreatedAt
                };
            }
        }
    }
}
