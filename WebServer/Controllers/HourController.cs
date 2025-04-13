using Application.DTOs;
using Application.Hours.Commands;
using Application.Hours.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HourController(IMediator mediator) : Controller
    {
        private readonly IMediator _mediator = mediator;

        [Authorize]
        [HttpGet()]
        public async Task<List<HourDto>> GetHourList()
        {
            return await _mediator.Send(new GetHourListQuery());
        }

        [Authorize]
        [HttpPost()]
        public async Task<Guid> HourCreate([FromBody] HourDto hour)
        {
            return await _mediator.Send(new HourCreateCommand(hour));
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<Unit> HourDelete([FromRoute] Guid id)
        {
            return await _mediator.Send(new HourDeleteCommand { Id = id });
        }
    }
}
