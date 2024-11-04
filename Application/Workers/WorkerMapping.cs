using Application.DTOs;
using Domain.Entities;
using System.Linq.Expressions;

namespace Application.Workers
{
    public static class WorkerMapping
    {
        public static Expression<Func<Worker, WorkerDto>> WorkerProjection
        {
            get
            {
                return u => new WorkerDto
                {
                    Id = u.Id,
                    Name = u.Name,
                    FirstName = u.FirstName,
                    Email = u.Email,
                    Specialization = u.Specialization,
                    Available = u.Available,
                    IsGuard = u.IsGuard,
                    Percent = u.Percent,
                    NoDaysVacation = u.NoDaysVacation
                };
            }
        }

        public static PercentDto PercentProjection(Percent percent)
        {
            return new PercentDto
            {
                Id = percent.Id,
                Value = percent.Value
            };
        }
    }
}
