using Application.DTOs;
using Application.Interfaces;
using MediatR;

namespace Application.Workers.Queries
{
    public class WorkerGetByEmailQuery : IRequest<WorkerDto>
    {
        public WorkerGetByEmailQuery(string email)
        {
            Email = email;
        }

        public string Email { get; set; }
    }

    public class WorkerGetByEmailQueryHandler : IRequestHandler<WorkerGetByEmailQuery, WorkerDto>
    {
        private readonly IAppDbContext _appDbContext;

        public WorkerGetByEmailQueryHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<WorkerDto> Handle(WorkerGetByEmailQuery request, CancellationToken cancellationToken)
        {
            var worker = _appDbContext.Workers.Where(p => p.Email == request.Email).Select(WorkerMapping.WorkerProjection).FirstOrDefault();
            if (worker != null) return worker;
            else return null;
        }
    }
}
