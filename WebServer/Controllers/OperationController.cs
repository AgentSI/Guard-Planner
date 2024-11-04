using Application.DTOs;
using Application.Interfaces.Pagination;
using Application.Operations.Commands;
using Application.Operations.Queries;
using Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OperationController : Controller
    {
        private readonly IMediator _mediator;
        private readonly AppDbContext _context;

        public OperationController(IMediator mediator, AppDbContext context)
        {
            _mediator = mediator;
            _context = context;
        }

        [Authorize]
        [HttpPost("all")]
        public async Task<PaginationResult<OperationDto>> OperationGetList(PaginationParameter paginationParameter)
        {
            return await _mediator.Send(new OperationListQuery(paginationParameter, null));
        }

        [Authorize]
        [HttpPost("all/{guardId}")]
        public async Task<PaginationResult<OperationDto>> OperationGetList(PaginationParameter paginationParameter, [FromRoute] Guid guardId)
        {
            return await _mediator.Send(new OperationListQuery(paginationParameter, guardId));
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<OperationDto> OperationGetById([FromRoute] Guid id)
        {
            return await _mediator.Send(new OperationGetByIdQuery(id));
        }

        [HttpPost("create")]
        public async Task<Guid> OperationCreate(OperationDto request)
        {
            return await _mediator.Send(new OperationCreateCommand(request));
        }

        [Authorize]
        [HttpPut("edit")]
        public async Task<Unit> OperationEdit([FromBody] OperationDto request)
        {
            return await _mediator.Send(new OperationEditCommand(request));
        }

        [Authorize]
        [HttpDelete("delete/{id}")]
        public async Task<Unit> OperationDelete([FromRoute] Guid id)
        {
            return await _mediator.Send(new OperationDeleteCommand { Id = id });
        }
    }
}
