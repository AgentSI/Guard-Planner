using Application.DTOs;
using Application.Menu.Commands;
using Application.Menu.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController(IMediator mediator) : Controller
    {
        private readonly IMediator _mediator = mediator;

        [Authorize]
        [HttpGet]
        public async Task<List<MenuDto>> GetMenu()
        {
            return await _mediator.Send(new GetMenuQuery());
        }

        [Authorize]
        [HttpPut]
        public async Task<ActionResult<MenuDto>> EditMenu([FromBody] MenuDto menuDto)
        {
            var result = await _mediator.Send(new EditMenuCommand { model = menuDto });
            return Ok(result);
        }
    }
}
