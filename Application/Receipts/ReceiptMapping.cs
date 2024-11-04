using Application.DTOs;
using Domain.Entities;
using System.Linq.Expressions;

namespace Application.Receipts
{
    public static class ReceiptMapping
    {
        public static Expression<Func<Receipt, ReceiptDto>> ReceiptProjection
        {
            get
            {
                return u => new ReceiptDto
                {
                    Id = u.Id,
                    Name = u.Name,
                    OperationId = u.OperationId
                };
            }
        }
    }
}
