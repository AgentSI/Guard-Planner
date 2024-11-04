using Application.DTOs;
using Application.Interfaces.Pagination;
using Application.Users.Commands;
using Application.Users.Queries;
using Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IMediator _mediator;
        private readonly AppDbContext _context;

        public UserController(IMediator mediator, AppDbContext context)
        {
            _mediator = mediator;
            _context = context;
        }

        [Authorize]
        [HttpPost("all")]
        public async Task<PaginationResult<UserDto>> UserGetList(PaginationParameter paginationParameter)
        {
            return await _mediator.Send(new UserListQuery(paginationParameter));
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<UserDto> UserGetById([FromRoute] Guid id)
        {
            return await _mediator.Send(new UserGetByIdQuery(id));
        }

        [HttpPost("create")]
        public async Task<IActionResult> UserCreate(UserDto request)
        {
            var result = await _mediator.Send(new UserCreateCommand(request));
            return Ok(result);
        }

        [Authorize]
        [HttpPut("edit")]
        public async Task<Unit> UserEdit([FromBody] UserDto request)
        {
            return await _mediator.Send(new UserEditCommand(request));
        }

        [Authorize]
        [HttpGet("roles")]
        public async Task<List<string>> GetRoles()
        {
            return await _mediator.Send(new GetRolesQuery());
        }

        [Authorize]
        [HttpDelete("delete/{id}")]
        public async Task<Unit> UserDelete([FromRoute] Guid id)
        {
            return await _mediator.Send(new UserDeleteCommand { Id = id });
        }

        [HttpGet("reset-password/{email}&{password}")]
        public async Task<Unit> UserResetPassword([FromRoute] string email, [FromRoute] string password)
        {
            return await _mediator.Send(new UserResetPasswordQuery(email, password));
        }

        [HttpGet("sendConfirmationCode/{email}")]
        public async Task<bool> SendConfirmationCode([FromRoute] string email)
        {
            return await _mediator.Send(new SendConfirmationCodeQuery(email));
        }

        [HttpGet("verifyConfirmationCode/{email}&{code}")]
        public async Task<bool> VerifyConfirmationCode([FromRoute] string email, [FromRoute] string code)
        {
            return await _mediator.Send(new VerifyConfirmationCodeQuery(email, code));
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResultDto>> Login(LoginDto request)
        {
            var command = new LoginCommand(request);
            var result = await _mediator.Send(command);
            if (result.Token != null)
            {
                HttpContext.Response.Cookies.Append("token", result.Token, new CookieOptions
                {
                    Expires = DateTime.Now.AddDays(365),
                    HttpOnly = false,
                    Secure = true,
                    IsEssential = true,
                    SameSite = SameSiteMode.None,
                });
            }

            return result;
        }

        [HttpGet("log-out")]
        [HttpPost("log-out")]
        [AllowAnonymous]
        public async Task<ActionResult<string>> LogOut()
        {
            Response.Cookies.Delete("token");
            return Ok("");
        }
    }
}
