using Application.DTOs;
using Application.Users.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace WebServer.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class TokenMeController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IConfiguration _configuration;

        public TokenMeController(IMediator mediator, IConfiguration configuration)
        {
            _mediator = mediator;
            _configuration = configuration;
        }

        [Authorize]
        [HttpGet("tokenme")]
        public async Task<ClaimsDto> Me()
        {
            var jwt = "";

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(_configuration.GetSection("AppSettings:Token").Value)),
                ValidateIssuer = false,
                ValidateAudience = false,
                RequireExpirationTime = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                ValidAlgorithms = new[] { SecurityAlgorithms.HmacSha512 }
            };
            var tokenHandler = new JwtSecurityTokenHandler();

            SecurityToken validatedToken;

            try
            {
                HttpContext.Request.Cookies.TryGetValue("token", out jwt);
                tokenHandler.ValidateToken(jwt, validationParameters, out validatedToken);
                var jwtToken = validatedToken as JwtSecurityToken;
                var userIdClaim = jwtToken?.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
                var user = await _mediator.Send(new UserGetByIdQuery(Guid.Parse(userIdClaim)));
                if (user == null)
                {
                    Response.Cookies.Delete("token");
                    return null;
                }
                else
                {
                    var claims = new ClaimsDto((validatedToken as JwtSecurityToken).Claims);
                    return claims;
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: ", e);
                return null;
            }
        }
    }
}
