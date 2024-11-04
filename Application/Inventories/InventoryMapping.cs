using Application.DTOs;
using Domain.Entities;
using System.Linq.Expressions;

namespace Application.Inventories
{
    public static class InventoryMapping
    {
        public static Expression<Func<Inventory, InventoryDto>> InventoryProjection
        {
            get
            {
                return u => new InventoryDto
                {
                    Id = u.Id,
                    Amount = u.Amount,
                    Name = u.Name,
                    Measure = u.Measure,
                    ReceiptId = u.ReceiptId
                };
            }
        }
    }
}
