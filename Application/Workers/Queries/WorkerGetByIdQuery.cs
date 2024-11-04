using Application.DTOs;
using Application.Interfaces;
using MediatR;

namespace Application.Workers.Queries
{
    public class WorkerGetByIdQuery : IRequest<WorkerDto>
    {
        public WorkerGetByIdQuery(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }

    public class WorkerGetByIdQueryHandler : IRequestHandler<WorkerGetByIdQuery, WorkerDto>
    {
        private readonly IAppDbContext _appDbContext;

        public WorkerGetByIdQueryHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<WorkerDto> Handle(WorkerGetByIdQuery request, CancellationToken cancellationToken)
        {
            var worker = _appDbContext.Workers.Where(p => p.Id == request.Id).Select(WorkerMapping.WorkerProjection).FirstOrDefault();
            if (worker != null) return worker;
            else return null;
        }
    }
}
