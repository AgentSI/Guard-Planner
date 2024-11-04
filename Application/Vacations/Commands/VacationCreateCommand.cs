using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;

namespace Application.Vacations.Commands
{
    public class VacationCreateCommand : IRequest<VacationCreateResultDto>
    {
        public VacationCreateCommand(VacationDto create)
        {
            WorkerName = create.WorkerName;
            StartDate = create.StartDate;
            EndDate = create.EndDate;
            Reason = create.Reason;
        }

        public string WorkerName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Reason { get; set; }
    }

    public class VacationCreateCommandHandler : IRequestHandler<VacationCreateCommand, VacationCreateResultDto>
    {
        private readonly IAppDbContext _appDbContext;

        public VacationCreateCommandHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<VacationCreateResultDto> Handle(VacationCreateCommand request, CancellationToken cancellationToken)
        {
            var worker = await _appDbContext.Workers.Include(w => w.Guards).Where(w => w.Email == request.WorkerName).FirstOrDefaultAsync();
            if (worker == null)
                return new VacationCreateResultDto { Success = false, ErrorMessage = "Lucrător-ul nu a fost găsit" };

            var existing = await _appDbContext.Vacations
                                                .FirstOrDefaultAsync(v => v.WorkerId == worker.Id &&
                                                    ((request.StartDate >= v.StartDate && request.StartDate <= v.EndDate) ||
                                                     (request.EndDate >= v.StartDate && request.EndDate <= v.EndDate) ||
                                                     (request.StartDate <= v.StartDate && request.EndDate >= v.EndDate)));
            if (existing != null)
                return new VacationCreateResultDto 
                { 
                    Success = false, 
                    ErrorMessage = $"Concedi-ul în perioada {existing.StartDate?.ToString("dd.MM.yyyy")} - {existing.EndDate?.ToString("dd.MM.yyyy")} pentru {worker.Name} există" 
                };

            var hasGuard = worker.Guards != null && worker.Guards.Any(g => g.Date >= request.StartDate && g.Date <= request.EndDate);
            if (hasGuard)
            {
                return new VacationCreateResultDto
                {
                    Success = false,
                    ErrorMessage = $"Concediul nu poate fi adăugat deoarece lucrătorul este în gardă"
                };
            }

            if (worker.NoDaysVacation == 0)
                return new VacationCreateResultDto { Success = false, ErrorMessage = "Nu sunt disponibile zile de concediu" };

            var requestedDays = (request.EndDate.Value - request.StartDate.Value).Days + 1;
            if (worker.NoDaysVacation < requestedDays)
                return new VacationCreateResultDto { Success = false, ErrorMessage = $"Zile de concediu disponibile: {worker.NoDaysVacation}" };

            var create = new Vacation
            {
                StartDate = request.StartDate ?? DateTime.Now,
                EndDate = request.EndDate ?? DateTime.Now,
                Reason = request.Reason,
                Worker = worker,
                WorkerId = worker.Id,
                NoDays = requestedDays
            };

            worker.NoDaysVacation -= create.NoDays;
            _appDbContext.Vacations.Add(create);
            await _appDbContext.SaveChangesAsync(cancellationToken);

            return new VacationCreateResultDto { Success = true, Id = create.Id };
        }
    }
}
