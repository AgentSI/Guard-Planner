using Application.DTOs;
using Application.Interfaces.Pagination;
using Application.Workers.Commands;
using Application.Workers.Queries;
using Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkerController : Controller
    {
        private readonly IMediator _mediator;
        private readonly AppDbContext _context;

        public WorkerController(IMediator mediator, AppDbContext context)
        {
            _mediator = mediator;
            _context = context;
        }

        [Authorize]
        [HttpPost("all")]
        public async Task<PaginationResult<WorkerDto>> WorkerGetList(PaginationParameter paginationParameter)
        {
            return await _mediator.Send(new WorkerListQuery(paginationParameter));
        }

        [Authorize]
        [HttpGet("workerhours/{month}/{year}")]
        public async Task<List<WorkerHoursDto>> GetWorkerHours([FromRoute] int month, [FromRoute] int year)
        {
            return await _mediator.Send(new GetWorkerHoursQuery(month, year));
        }

        [Authorize]
        [HttpGet("workers")]
        public async Task<List<WorkerDto>> GetWorkers()
        {
            return await _mediator.Send(new GetWorkersQuery());
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<WorkerDto> WorkerGetById([FromRoute] Guid id)
        {
            return await _mediator.Send(new WorkerGetByIdQuery(id));
        }

        [Authorize]
        [HttpGet("email={email}")]
        public async Task<WorkerDto> GetWorkerByEmail([FromRoute] string email)
        {
            return await _mediator.Send(new WorkerGetByEmailQuery(email));
        }

        [HttpPost("create")]
        public async Task<Guid> WorkerCreate(WorkerDto request)
        {
            return await _mediator.Send(new WorkerCreateCommand(request));
        }

        [Authorize]
        [HttpPut("edit")]
        public async Task<Unit> WorkerEdit([FromBody] WorkerDto request)
        {
            return await _mediator.Send(new WorkerEditCommand(request));
        }

        [Authorize]
        [HttpDelete("delete/{id}")]
        public async Task<Unit> WorkerDelete([FromRoute] Guid id)
        {
            return await _mediator.Send(new WorkerDeleteCommand { Id = id });
        }

        [HttpGet("percentages")]
        public async Task<List<double>> GetPercentages()
        {
            return await _mediator.Send(new GetPercentagesQuery());
        }

        [HttpGet("percentList")]
        public async Task<List<PercentDto>> GetPercentList()
        {
            return await _mediator.Send(new GetPercentListQuery());
        }

        [HttpPost("PercentCreate")]
        public async Task<Guid> PercentCreate([FromBody] double percent)
        {
            return await _mediator.Send(new PercentCreateCommand(percent));
        }

        [Authorize]
        [HttpDelete("PercentDelete/{id}")]
        public async Task<Unit> PercentDelete([FromRoute] Guid id)
        {
            return await _mediator.Send(new PercentDeleteCommand { Id = id });
        }
    }
}
