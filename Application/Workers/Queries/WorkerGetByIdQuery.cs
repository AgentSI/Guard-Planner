using Application.DTOs;
using Application.Interfaces;
using MediatR;

namespace Application.Workers.Queries
{
    public class WorkerGetByIdQuery(Guid id) : IRequest<WorkerDto>
    {
        public Guid Id { get; set; } = id;
    }

    public class WorkerGetByIdQueryHandler(IAppDbContext appDbContext) : IRequestHandler<WorkerGetByIdQuery, WorkerDto>
    {
        private readonly IAppDbContext _appDbContext = appDbContext;

        public Task<WorkerDto> Handle(WorkerGetByIdQuery request, CancellationToken cancellationToken)
        {
            var worker = _appDbContext.Workers.Where(p => p.Id == request.Id).Select(WorkerMapping.WorkerProjection).FirstOrDefault();
            if (worker != null) return Task.FromResult(worker);
            else return Task.FromResult(new WorkerDto { });
        }
    }
}
