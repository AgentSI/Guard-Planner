using Application.DTOs;
using Domain.Entities;
using System.Linq.Expressions;

namespace Application.Menu
{
    public static class MenuMapping
    {
        public static Expression<Func<MenuItem, MenuDto>> MenuProjection
        {
            get
            {
                return u => new MenuDto
                {
                    Id = u.Id,
                    OriginalName = u.OriginalName,
                    DisplayName = u.DisplayName,
                    IsChecked = u.IsChecked,
                    Href = u.Href,
                    Icon = u.Icon
                };
            }
        }
    }
}
