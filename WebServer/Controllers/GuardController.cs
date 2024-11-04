using Application.DTOs;
using Application.Interfaces.Pagination;
using Application.Guards.Commands;
using Application.Guards.Queries;
using Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GuardController : Controller
    {
        private readonly IMediator _mediator;
        private readonly AppDbContext _context;

        public GuardController(IMediator mediator, AppDbContext context)
        {
            _mediator = mediator;
            _context = context;
        }

        [Authorize]
        [HttpGet("all/{month}/{year}")]
        public async Task<List<GuardDto>> GuardsGet([FromRoute] int month, [FromRoute] int year)
        {
            return await _mediator.Send(new GuardsGetQuery(month, year));
        }

        [Authorize]
        [HttpPost("all")]
        public async Task<PaginationResult<GuardDto>> GuardGetList(PaginationParameter paginationParameter)
        {
            return await _mediator.Send(new GuardListQuery(paginationParameter, null));
        }

        [Authorize]
        [HttpPost("all/{workerId}")]
        public async Task<PaginationResult<GuardDto>> GuardGetList(PaginationParameter paginationParameter, [FromRoute] Guid workerId)
        {
            return await _mediator.Send(new GuardListQuery(paginationParameter, workerId));
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<GuardDto> GuardGetById([FromRoute] Guid id)
        {
            return await _mediator.Send(new GuardGetByIdQuery(id));
        }

        [HttpPost("create")]
        public async Task<Guid> GuardCreate(GuardDto request)
        {
            return await _mediator.Send(new GuardCreateCommand(request));
        }

        [Authorize]
        [HttpPut("edit")]
        public async Task<Unit> GuardEdit([FromBody] GuardDto request)
        {
            return await _mediator.Send(new GuardEditCommand(request));
        }

        [Authorize]
        [HttpDelete("delete/{id}")]
        public async Task<Unit> GuardDelete([FromRoute] Guid id)
        {
            return await _mediator.Send(new GuardDeleteCommand { Id = id });
        }

        [Authorize]
        [HttpGet("{month}/{year}")]
        public async Task<List<GuardDto>> GenerateGuards([FromRoute] int month, [FromRoute] int year)
        {
            return await _mediator.Send(new GenerateGuardsQuery(month, year));
        }

        [HttpPost("save")]
        public async Task<Unit> SaveGuards(List<GuardDto> guards)
        {
            return await _mediator.Send(new SaveGuardsCommand(guards));
        }

        [HttpPost("send")]
        public async Task<Unit> SendGuards(List<GuardDto> guards)
        {
            return await _mediator.Send(new SendGuardsCommand(guards));
        }

        [HttpPost("deleteGuards")]
        public async Task<Unit> DeleteGuards([FromBody] DateTime? date)
        {
            return await _mediator.Send(new DeleteGuardsCommand(date));
        }
    }
}
