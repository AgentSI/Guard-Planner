using Application.DTOs;
using Domain.Entities;
using System.Linq.Expressions;

namespace Application.Guards
{
    public static class GuardMapping
    {
        public static Expression<Func<Guard, GuardDto>> GuardProjection
        {
            get
            {
                return u => new GuardDto
                {
                    Id = u.Id,
                    Date = u.Date,
                    Hours = (int)u.Hours,
                    WorkerId = u.WorkerId,
                    NrOperations = u.Operations.Count(),
                    WorkerName = u.Worker.Name + " " + u.Worker.FirstName
                };
            }
        }
    }
}
