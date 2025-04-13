using Application.DTOs;
using Application.Interfaces.Pagination;
using Application.Instruments.Commands;
using Application.Instruments.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstrumentController(IMediator mediator) : Controller
    {
        private readonly IMediator _mediator = mediator;

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
