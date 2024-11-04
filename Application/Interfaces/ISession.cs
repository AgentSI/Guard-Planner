using Application.DTOs;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces
{
    public interface ISession
    {
        CurrentUserDto CurrentUser { get; set; }
        IHttpContextAccessor HttpContextAccessor { get; set; }
    }
}
