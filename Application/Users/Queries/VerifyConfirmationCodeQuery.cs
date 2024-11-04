using Application.Interfaces;
using MediatR;

namespace Application.Users.Queries
{
    public class VerifyConfirmationCodeQuery : IRequest<bool>
    {
        public VerifyConfirmationCodeQuery(string email, string code)
        {
            Email = email;
            Code = code;
        }

        public string Email { get; set; }
        public string Code { get; set; }
    }

    public class VerifyConfirmationCodeQueryHandler : IRequestHandler<VerifyConfirmationCodeQuery, bool>
    {
        private readonly IAppDbContext _appDbContext;

        public VerifyConfirmationCodeQueryHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<bool> Handle(VerifyConfirmationCodeQuery request, CancellationToken cancellationToken)
        {
            var user = _appDbContext.Users.Where(p => p.Email == request.Email).FirstOrDefault();
            if (user.SecurityCode == request.Code) return true;
            else return false;
        }
    }
}
