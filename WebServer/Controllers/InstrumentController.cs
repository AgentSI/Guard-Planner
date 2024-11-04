using Application.DTOs;
using Application.Interfaces.Pagination;
using Application.Instruments.Commands;
using Application.Instruments.Queries;
using Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstrumentController : Controller
    {
        private readonly IMediator _mediator;
        private readonly AppDbContext _context;

        public InstrumentController(IMediator mediator, AppDbContext context)
        {
            _mediator = mediator;
            _context = context;
        }

        [Authorize]
        [HttpPost("all/{receiptId}")]
        public async Task<PaginationResult<InstrumentDto>> InventoryGetList(PaginationParameter paginationParameter, [FromRoute] Guid receiptId)
        {
            return await _mediator.Send(new InstrumentListQuery(paginationParameter, receiptId));
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<InstrumentDto> InstrumentGetById([FromRoute] Guid id)
        {
            return await _mediator.Send(new InstrumentGetByIdQuery(id));
        }

        [HttpPost("create")]
        public async Task<Guid> InstrumentCreate(InstrumentDto request)
        {
            return await _mediator.Send(new InstrumentCreateCommand(request));
        }

        [Authorize]
        [HttpPut("edit")]
        public async Task<Unit> InstrumentEdit([FromBody] InstrumentDto request)
        {
            return await _mediator.Send(new InstrumentEditCommand(request));
        }

        [Authorize]
        [HttpDelete("delete/{id}")]
        public async Task<Unit> InstrumentDelete([FromRoute] Guid id)
        {
            return await _mediator.Send(new InstrumentDeleteCommand { Id = id });
        }
    }
}
