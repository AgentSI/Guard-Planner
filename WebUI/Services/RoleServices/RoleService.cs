using Application.DTOs;
using Application.Interfaces.Pagination;
using MediatR;
using MudBlazor;
using System.Net.Http.Json;

namespace WebUI.Services.RoleServices
{
    public class RoleService(HttpClient httpClient, ISnackbar snackbar) : IRoleService
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly ISnackbar _snackbar = snackbar;

        public async Task<PaginationResult<UserRoleDto>> GetRoles(PaginationParameter paginationParameter)
        {
            var result = await _httpClient.PostAsJsonAsync($"api/Role/all", paginationParameter);
            if (!result.IsSuccessStatusCode) return default;

            return await result.Content.ReadFromJsonAsync<PaginationResult<UserRoleDto>>();
        }

        public async Task<Unit> RoleCreate(UserRoleDto request)
        {
            var result = await _httpClient.PostAsJsonAsync("api/Role/create", request);
            if (result.IsSuccessStatusCode)
            {
                var id = await result.Content.ReadFromJsonAsync<Unit>();
                _snackbar.Add("Rolul a fost creat.", Severity.Success);
                return id;
            }
            _snackbar.Add("A apărut o eroare...", Severity.Error);
            return default;
        }

        public async Task<Unit> RoleDelete(Guid id)
        {
            var result = await _httpClient.DeleteAsync($"api/Role/delete/{id}");
            if (result.IsSuccessStatusCode)
            {
                _snackbar.Add("Rolul a fost șters.", Severity.Success);
                return await result.Content.ReadFromJsonAsync<Unit>();
            }
            _snackbar.Add("A apărut o eroare...", Severity.Error);
            return default;
        }

        public async Task<Unit> RoleEdit(UserRoleDto request)
        {
            var result = await _httpClient.PutAsJsonAsync("api/Role/edit", request);
            if (result.IsSuccessStatusCode)
            {
                _snackbar.Add("Rolul a fost editat.", Severity.Success);
                return await result.Content.ReadFromJsonAsync<Unit>();
            }
            _snackbar.Add("A apărut o eroare...", Severity.Error);
            return default;
        }

        public async Task<UserRoleDto> GetRoleById(Guid id)
        {
            var result = await _httpClient.GetAsync($"api/Role/{id}");
            return await result.Content.ReadFromJsonAsync<UserRoleDto>();
        }
    }
}
