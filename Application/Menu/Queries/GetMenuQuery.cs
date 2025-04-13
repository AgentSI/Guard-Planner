using Application.DTOs;
using Application.Interfaces;
using MediatR;

namespace Application.Menu.Queries
{
    public class GetMenuQuery() : IRequest<List<MenuDto>> { }

    public class GetMenuQueryHandler(IAppDbContext appDbContext) : IRequestHandler<GetMenuQuery, List<MenuDto>>
    {
        private readonly IAppDbContext _appDbContext = appDbContext;

        public Task<List<MenuDto>> Handle(GetMenuQuery request, CancellationToken cancellationToken)
        {
            var menu = _appDbContext.MenuItems.Select(MenuMapping.MenuProjection).ToList();
            if (menu != null) return Task.FromResult(menu);
            else return Task.FromResult(new List<MenuDto> { });
        }
    }
}
