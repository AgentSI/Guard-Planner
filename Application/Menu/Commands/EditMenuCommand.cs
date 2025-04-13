using Application.DTOs;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Menu.Commands
{
    public class EditMenuCommand() : IRequest<MenuDto>
    {
        public MenuDto? model { get; set; }
    }

    public class EditMenuCommandHandler(IAppDbContext appDbContext) : IRequestHandler<EditMenuCommand, MenuDto>
    {
        private readonly IAppDbContext _appDbContext = appDbContext;

        public async Task<MenuDto> Handle(EditMenuCommand request, CancellationToken cancellationToken)
        {
            var menuDto = request.model;

            var menuItem = await _appDbContext.MenuItems.FirstOrDefaultAsync(m => m.Id == menuDto!.Id, cancellationToken);

            if (menuItem == null) return new MenuDto { };

            menuItem.OriginalName = menuDto!.OriginalName;
            menuItem.DisplayName = menuDto.DisplayName;
            menuItem.IsChecked = menuDto.IsChecked;

            await _appDbContext.SaveChangesAsync(cancellationToken);

            return new MenuDto
            {
                Id = menuItem.Id,
                OriginalName = menuItem.OriginalName,
                DisplayName = menuItem.DisplayName,
                IsChecked = menuItem.IsChecked
            };
        }
    }
}
