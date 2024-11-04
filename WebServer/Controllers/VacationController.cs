using Application.DTOs;
using Application.Interfaces.Pagination;
using Application.Vacations.Commands;
using Application.Vacations.Queries;
using Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VacationController : Controller
    {
        private readonly IMediator _mediator;
        private readonly AppDbContext _context;

        public VacationController(IMediator mediator, AppDbContext context)
        {
            _mediator = mediator;
            _context = context;
        }

        [Authorize]
        [HttpPost("all")]
        public async Task<PaginationResult<VacationDto>> VacationGetList(PaginationParameter paginationParameter)
        {
            return await _mediator.Send(new VacationListQuery(paginationParameter, null));
        }

        [Authorize]
        [HttpPost("all/{workerId}")]
        public async Task<PaginationResult<VacationDto>> VacationGetList(PaginationParameter paginationParameter, [FromRoute] Guid workerId)
        {
            return await _mediator.Send(new VacationListQuery(paginationParameter, workerId));
        }

        [Authorize]
        [HttpGet("{month}/{year}")]
        public async Task<List<VacationDto>> VacationsForMonth(int month, int year)
        {
            return await _mediator.Send(new VacationsForMonthQuery(month, year));
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<VacationDto> VacationGetById([FromRoute] Guid id)
        {
            return await _mediator.Send(new VacationGetByIdQuery(id));
        }

        [HttpPost("create")]
        public async Task<VacationCreateResultDto> VacationCreate(VacationDto request)
        {
            return await _mediator.Send(new VacationCreateCommand(request));
        }

        [Authorize]
        [HttpPut("edit")]
        public async Task<Unit> VacationEdit([FromBody] VacationDto request)
        {
            return await _mediator.Send(new VacationEditCommand(request));
        }

        [Authorize]
        [HttpDelete("delete/{id}")]
        public async Task<Unit> VacationDelete([FromRoute] Guid id)
        {
            return await _mediator.Send(new VacationDeleteCommand { Id = id });
        }
    }
}
