using Application.DTOs;
using Application.Receipts.Commands;
using Application.Receipts.Queries;
using Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReceiptController : Controller
    {
        private readonly IMediator _mediator;
        private readonly AppDbContext _context;

        public ReceiptController(IMediator mediator, AppDbContext context)
        {
            _mediator = mediator;
            _context = context;
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ReceiptDto> ReceiptGetById([FromRoute] Guid id)
        {
            return await _mediator.Send(new ReceiptGetByIdQuery(id));
        }

        [HttpPost("create")]
        public async Task<Guid> ReceiptCreate(ReceiptDto request)
        {
            return await _mediator.Send(new ReceiptCreateCommand(request));
        }

        [Authorize]
        [HttpPut("edit")]
        public async Task<Unit> ReceiptEdit([FromBody] ReceiptDto request)
        {
            return await _mediator.Send(new ReceiptEditCommand(request));
        }

        [Authorize]
        [HttpDelete("delete/{id}")]
        public async Task<Unit> ReceiptDelete([FromRoute] Guid id)
        {
            return await _mediator.Send(new ReceiptDeleteCommand { Id = id });
        }
    }
}
