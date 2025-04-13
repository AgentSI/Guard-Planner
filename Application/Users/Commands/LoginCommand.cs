using Application.DTOs;
using Application.Interfaces;
using Domain;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Application.Users.Commands
{
    public class LoginCommand(LoginDto postModel) : IRequest<AuthResultDto>
    {
        public string? Email { get; set; } = postModel.Email;
        public string? Password { get; set; } = postModel.Password;
        public string? FireBaseTokenId { get; set; }
    }

    public class LoginCommandHandler(IAppDbContext appDbContext, IConfiguration configuration) : IRequestHandler<LoginCommand, AuthResultDto>
    {
        private readonly IAppDbContext _appDbContext = appDbContext;
        private readonly IConfiguration _configuration = configuration;

        public async Task<AuthResultDto> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            AuthResultDto response = new AuthResultDto();

            var appUser = await _appDbContext.Users.Include(u => u.UserRole).FirstOrDefaultAsync(d => d.Email == request.Email);

            if (appUser == null)
            {
                response.ResponseMessage = "Credențiale greșite!";
                response.Success = false;
            }
            else
            {
                if (Crypto.VerifyHashedPassword(appUser.PasswordHash!, AuthorizationVariables.Salt + request.Password))
                {
                    response.Token = CreateToken(appUser);
                    response.Success = true;
                    response.ResponseMessage = "Login Success";
                    await _appDbContext.SaveChangesAsync(cancellationToken);
                }
                else
                {
                    response.ResponseMessage = "Credențiale greșite!";
                    response.Success = false;
                }
            }

            return response;
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim("userId", user.Id.ToString()),
                new Claim("role", user.UserRole!.RoleName!.ToString()),
                new Claim("email", user.Email!),
            };

            var key = new SymmetricSecurityKey(Convert.FromBase64String(_configuration.GetSection("AppSettings:Token").Value!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);
            var token = new JwtSecurityToken
                (
                    claims: claims,
                    expires: DateTime.Now.AddDays(30),
                    signingCredentials: creds
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    }
}
