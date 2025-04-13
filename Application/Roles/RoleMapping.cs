using Application.DTOs;
using Domain.Entities;
using System.Linq.Expressions;

namespace Application.Roles
{
    public static class RoleMapping
    {
        public static Expression<Func<UserRole, UserRoleDto>> UserRoleProjection
        {
            get
            {
                return u => new UserRoleDto
                {
                    Id = u.Id,
                    RoleName = u.RoleName
                };
            }
        }
    }
}
