using Application.DTOs;
using Domain.Entities;
using System.Linq.Expressions;

namespace Application.Instruments
{
    public static class InstrumentMapping
    {
        public static Expression<Func<Instrument, InstrumentDto>> InstrumentProjection
        {
            get
            {
                return u => new InstrumentDto
                {
                    Id = u.Id,
                    Amount = u.Amount,
                    Name = u.Name,
                    ReceiptId = u.ReceiptId
                };
            }
        }
    }
}
