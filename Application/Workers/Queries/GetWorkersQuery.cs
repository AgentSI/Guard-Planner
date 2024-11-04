using Application.DTOs;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Workers.Queries
{
    public class GetWorkersQuery : IRequest<List<WorkerDto>>
    {
    }

    public class GetUsersQueryHandler : IRequestHandler<GetWorkersQuery, List<WorkerDto>>
    {
        private readonly IAppDbContext _appDbContext;
        public GetUsersQueryHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<List<WorkerDto>> Handle(GetWorkersQuery request, CancellationToken cancellationToken)
        {
            var workers = await _appDbContext.Workers.ToListAsync();
            var workerDtos = workers.Select(w => new WorkerDto
            {
                Id = w.Id,
                Name = w.Name,
                Email = w.Email,
                Percent = w.Percent
            }).ToList();

            return workerDtos;
        }
    }
}
