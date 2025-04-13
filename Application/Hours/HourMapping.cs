using Application.DTOs;
using Domain.Entities;

namespace Application.Workers
{
    public static class HourMapping
    {
        public static HourDto HourProjection(Hour hour)
        {
            return new HourDto
            {
                Id = hour.Id,
                Value = hour.Value,
                Label = hour.Label
            };
        }
    }
}
