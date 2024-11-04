using Application.DTOs;
using Application.Interfaces.Pagination;
using MediatR;
using MudBlazor;
using System.Net.Http.Json;

namespace WebUI.Services.InventoryServices
{
    public class InventoryService : IInventoryService
    {
        private readonly HttpClient _httpClient;
        private readonly ISnackbar _snackbar;

        public InventoryService(HttpClient httpClient, ISnackbar snackbar)
        {
            _httpClient = httpClient;
            _snackbar = snackbar;
        }

        public async Task<PaginationResult<InventoryDto>> GetInventories(PaginationParameter paginationParameter, Guid guardId)
        {
            var result = await _httpClient.PostAsJsonAsync($"api/Inventory/all/{guardId}", paginationParameter);
            if (!result.IsSuccessStatusCode) return default;

            return await result.Content.ReadFromJsonAsync<PaginationResult<InventoryDto>>();
        }

        public async Task<Guid> InventoryCreate(InventoryDto request)
        {
            var result = await _httpClient.PostAsJsonAsync("api/Inventory/create", request);
            if (result.IsSuccessStatusCode)
            {
                var id = await result.Content.ReadFromJsonAsync<Guid>();
                if (id == Guid.Empty)
                {
                    _snackbar.Add("A apărut o eroare...", Severity.Error);
                    return default;
                }
                else
                {
                    _snackbar.Add("Inventarul a fost creat.", Severity.Success);
                    return id;
                }
            }
            _snackbar.Add("A apărut o eroare...", Severity.Error);
            return default;
        }

        public async Task<Unit> InventoryDelete(Guid id)
        {
            var result = await _httpClient.DeleteAsync($"api/Inventory/delete/{id}");
            if (result.IsSuccessStatusCode)
            {
                _snackbar.Add("Inventarul a fost șters.", Severity.Success);
                return await result.Content.ReadFromJsonAsync<Unit>();
            }
            _snackbar.Add("A apărut o eroare...", Severity.Error);
            return default;
        }

        public async Task<Unit> InventoryEdit(InventoryDto request)
        {
            var result = await _httpClient.PutAsJsonAsync("api/Inventory/edit", request);
            if (result.IsSuccessStatusCode)
            {
                _snackbar.Add("Inventarul a fost editat.", Severity.Success);
                return await result.Content.ReadFromJsonAsync<Unit>();
            }
            _snackbar.Add("A apărut o eroare...", Severity.Error);
            return default;
        }

        public async Task<InventoryDto> GetInventoryById(Guid id)
        {
            var result = await _httpClient.GetAsync($"api/Inventory/{id}");
            return await result.Content.ReadFromJsonAsync<InventoryDto>();
        }
    }
}
