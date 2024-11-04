using Application.DTOs;
using Application.Interfaces.Pagination;
using Application.Inventories.Commands;
using Application.Inventories.Queries;
using Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : Controller
    {
        private readonly IMediator _mediator;
        private readonly AppDbContext _context;

        public InventoryController(IMediator mediator, AppDbContext context)
        {
            _mediator = mediator;
            _context = context;
        }

        [Authorize]
        [HttpPost("all/{receiptId}")]
        public async Task<PaginationResult<InventoryDto>> InventoryGetList(PaginationParameter paginationParameter, [FromRoute] Guid receiptId)
        {
            return await _mediator.Send(new InventoryListQuery(paginationParameter, receiptId));
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<InventoryDto> InventoryGetById([FromRoute] Guid id)
        {
            return await _mediator.Send(new InventoryGetByIdQuery(id));
        }

        [HttpPost("create")]
        public async Task<Guid> InventoryCreate(InventoryDto request)
        {
            return await _mediator.Send(new InventoryCreateCommand(request));
        }

        [Authorize]
        [HttpPut("edit")]
        public async Task<Unit> InventoryEdit([FromBody] InventoryDto request)
        {
            return await _mediator.Send(new InventoryEditCommand(request));
        }

        [Authorize]
        [HttpDelete("delete/{id}")]
        public async Task<Unit> InventoryDelete([FromRoute] Guid id)
        {
            return await _mediator.Send(new InventoryDeleteCommand { Id = id });
        }
    }
}
