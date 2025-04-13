using Application.DTOs;
using MudBlazor;
using System.Net.Http.Json;

namespace WebUI.Services.MenuServices
{
    public class MenuService(HttpClient httpClient, ISnackbar snackbar) : IMenuService
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly ISnackbar _snackbar = snackbar;

        public async Task<List<MenuDto>> GetMenu()
        {
            var result = await _httpClient.GetAsync("api/Menu");
            if (!result.IsSuccessStatusCode) return default;

            return await result.Content.ReadFromJsonAsync<List<MenuDto>>();
        }

        public async Task<MenuDto> EditMenu(MenuDto menuDto)
        {
            var result = await _httpClient.PutAsJsonAsync($"api/Menu", menuDto);
            if (!result.IsSuccessStatusCode)
            {
                _snackbar.Add("Eroare la editarea meniului.", Severity.Error);
                return default;
            }

            _snackbar.Add("Meniu editat cu succes.", Severity.Success);
            return await result.Content.ReadFromJsonAsync<MenuDto>();
        }
    }
}
