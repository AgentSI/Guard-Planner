using Application.DTOs;
using Microsoft.AspNetCore.Components.Authorization;

namespace WebUI.Services.UserAccountServices
{
    public class CurrentUserService
    {
        private readonly AuthenticationStateProvider authStateProvider;
        private readonly CurrentUserDto currentUser;

        public CurrentUserService(AuthenticationStateProvider authStateProvider, CurrentUserDto currentUser)
        {
            this.authStateProvider = authStateProvider;
            this.currentUser = currentUser;
        }

        public async Task<CurrentUserDto> GetAsync()
        {
            var authState = await authStateProvider.GetAuthenticationStateAsync();
            var claims = authState.User?.Claims;

            if (claims != null && claims.Any())
            {
                var userIdClaim = claims.FirstOrDefault(c => c.Type == ClaimsDto.Id);
                var userRoleClaim = claims.FirstOrDefault(c => c.Type == ClaimsDto.UserRole);
                var userEmailClaim = claims.FirstOrDefault(c => c.Type == ClaimsDto.UserEmail);

                var userId = userIdClaim != null ? Guid.Parse(userIdClaim.Value) : Guid.Empty;
                var userRole = userRoleClaim != null ? userRoleClaim.Value : string.Empty;
                var userEmail = userEmailClaim != null ? userEmailClaim.Value : string.Empty;

                return new CurrentUserDto
                {
                    IsAuthenticated = true,
                    Id = userId,
                    UserRole = userRole,
                    Email = userEmail
                };
            }
            else
            {
                return new CurrentUserDto();
            }
        }

        public async Task InitializeAsync()
        {
            var authState = await authStateProvider.GetAuthenticationStateAsync();
            var claims = authState.User?.Claims.ToDictionary(d => d.Type, t => t.Value);

            if (claims != null && claims.Any())
            {
                currentUser.IsAuthenticated = true;
                currentUser.Id = Guid.Parse(claims[ClaimsDto.Id]);
                currentUser.UserRole = claims["role"];
            }
            else
            {
                currentUser.IsAuthenticated = false;
            }

        }
    }
}
