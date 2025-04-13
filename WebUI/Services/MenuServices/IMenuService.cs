using Application.DTOs;

namespace WebUI.Services.MenuServices
{
    public interface IMenuService
    {
        Task<List<MenuDto>> GetMenu();
        Task<MenuDto> EditMenu(MenuDto menuDto);
    }
}
