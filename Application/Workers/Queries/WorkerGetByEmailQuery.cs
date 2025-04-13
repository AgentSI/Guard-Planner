using Application.DTOs;
using Application.Interfaces;
using MediatR;

namespace Application.Workers.Queries
{
    public class WorkerGetByEmailQuery(string email) : IRequest<WorkerDto>
    {
        public string Email { get; set; } = email;
    }

    public class WorkerGetByEmailQueryHandler(IAppDbContext appDbContext) : IRequestHandler<WorkerGetByEmailQuery, WorkerDto>
    {
        private readonly IAppDbContext _appDbContext = appDbContext;

        public Task<WorkerDto> Handle(WorkerGetByEmailQuery request, CancellationToken cancellationToken)
        {
            var worker = _appDbContext.Workers.Where(p => p.Email == request.Email).Select(WorkerMapping.WorkerProjection).FirstOrDefault();
            if (worker != null) return Task.FromResult(worker);
            else return Task.FromResult(new WorkerDto());
        }
    }
}
