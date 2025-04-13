using Application.DTOs;
using Application.Interfaces.Pagination;
using Application.Roles.Commands;
using Application.Roles.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController(IMediator mediator) : Controller
    {
        private readonly IMediator _mediator = mediator;

        [Authorize]
        [HttpPost("all")]
        public async Task<PaginationResult<UserRoleDto>> UserRoleGetList(PaginationParameter paginationParameter)
        {
            return await _mediator.Send(new RoleListQuery(paginationParameter));
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<UserRoleDto> UserRoleGetById([FromRoute] Guid id)
        {
            return await _mediator.Send(new RoleGetByIdQuery(id));
        }

        [HttpPost("create")]
        public async Task<Unit> UserRoleCreate(UserRoleDto request)
        {
            return await _mediator.Send(new RoleCreateCommand(request));
        }

        [Authorize]
        [HttpPut("edit")]
        public async Task<Unit> UserRoleEdit([FromBody] UserRoleDto request)
        {
            return await _mediator.Send(new RoleEditCommand(request));
        }

        [Authorize]
        [HttpDelete("delete/{id}")]
        public async Task<Unit> UserRoleDelete([FromRoute] Guid id)
        {
            return await _mediator.Send(new RoleDeleteCommand { Id = id });
        }
    }
}
