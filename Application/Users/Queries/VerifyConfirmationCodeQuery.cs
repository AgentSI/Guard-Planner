using Application.Interfaces;
using MediatR;

namespace Application.Users.Queries
{
    public class VerifyConfirmationCodeQuery(string email, string code) : IRequest<bool>
    {
        public string Email { get; set; } = email;
        public string Code { get; set; } = code;
    }

    public class VerifyConfirmationCodeQueryHandler(IAppDbContext appDbContext) : IRequestHandler<VerifyConfirmationCodeQuery, bool>
    {
        private readonly IAppDbContext _appDbContext = appDbContext;

        public Task<bool> Handle(VerifyConfirmationCodeQuery request, CancellationToken cancellationToken)
        {
            var user = _appDbContext.Users.Where(p => p.Email == request.Email).FirstOrDefault();
            if (user!.SecurityCode == request.Code) return Task.FromResult(true);
            else return Task.FromResult(false);
        }
    }
}
