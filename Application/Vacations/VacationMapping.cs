using Application.DTOs;
using Domain.Entities;
using System.Linq.Expressions;

namespace Application.Vacations
{
    public static class VacationMapping
    {
        public static Expression<Func<Vacation, VacationDto>> VacationProjection
        {
            get
            {
                return u => new VacationDto
                {
                    Id = u.Id,
                    StartDate = u.StartDate,
                    EndDate = u.EndDate,
                    Reason = u.Reason,
                    WorkerId = u.WorkerId,
                    WorkerName = u.Worker.Name + " " + u.Worker.FirstName,
                    NoDays = u.NoDays
                };
            }
        }
    }
}
