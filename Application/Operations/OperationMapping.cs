using Application.DTOs;
using Domain.Entities;
using System.Linq.Expressions;

namespace Application.Operations
{
    public static class OperationMapping
    {
        public static Expression<Func<Operation, OperationDto>> OperationProjection
        {
            get
            {
                return u => new OperationDto
                {
                    Id = u.Id,
                    Type = u.Type,
                    StartTime = u.StartTime,
                    EndTime = u.EndTime,
                    ReceiptId = u.ReceiptId,
                    GuardId = u.GuardId,
                    Date = u.Guard.Date
                };
            }
        }
    }
}
