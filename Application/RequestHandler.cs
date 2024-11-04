using Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;

namespace Application
{
    public abstract class RequestHandler<TRequest, TResponse> : Session, IRequestHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        protected RequestHandler(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public abstract Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
    }

    public class Session : Interfaces.ISession
    {
        public IHttpContextAccessor HttpContextAccessor { get; set; }
        public CurrentUserDto CurrentUser { get; set; }
        public string HostName { get; set; }

        private IConfiguration Configuration { get; set; }

        public Session(IServiceProvider serviceProvider)
        {
            Configuration = (IConfiguration)serviceProvider.GetService(typeof(IConfiguration));
            HttpContextAccessor = (IHttpContextAccessor)serviceProvider.GetService(typeof(IHttpContextAccessor));

            CurrentUser = GetCurrentUser();

            HostName = Configuration.GetSection("HostName").Value;
        }

        private CurrentUserDto GetCurrentUser()
        {
            var principal = HttpContextAccessor?.HttpContext?.User;

            if (principal == null)
            {
                throw new ApplicationException("Principal null!.");
            }

            var email = principal.FindFirst(ClaimTypes.Email);
            var role = principal.FindFirst(ClaimTypes.Role);
            var id = principal.FindFirst("userId");

            if (string.IsNullOrEmpty(id?.Value))
            {
                return null;
            }

            var user = new CurrentUserDto
            {
                Id = Guid.Parse(id.Value),
                Email = email.Value,
                UserRole = role.Value
            };

            return user;
        }
    }
}
