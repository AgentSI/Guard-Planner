using Application.DTOs;
using Application.Interfaces.Pagination;
using MediatR;
using MudBlazor;
using System.Net.Http.Json;

namespace WebUI.Services.OperationServices
{
    public class OperationService : IOperationService
    {
        private readonly HttpClient _httpClient;
        private readonly ISnackbar _snackbar;

        public OperationService(HttpClient httpClient, ISnackbar snackbar)
        {
            _httpClient = httpClient;
            _snackbar = snackbar;
        }

        public async Task<PaginationResult<OperationDto>> GetOperations(PaginationParameter paginationParameter)
        {
            var result = await _httpClient.PostAsJsonAsync($"api/Operation/all", paginationParameter);
            if (!result.IsSuccessStatusCode) return default;

            return await result.Content.ReadFromJsonAsync<PaginationResult<OperationDto>>();
        }

        public async Task<PaginationResult<OperationDto>> GetOperations(PaginationParameter paginationParameter, Guid guardId)
        {
            var result = await _httpClient.PostAsJsonAsync($"api/Operation/all/{guardId}", paginationParameter);
            if (!result.IsSuccessStatusCode) return default;

            return await result.Content.ReadFromJsonAsync<PaginationResult<OperationDto>>();
        }

        public async Task<Guid> OperationCreate(OperationDto request)
        {
            var result = await _httpClient.PostAsJsonAsync("api/Operation/create", request);
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
                    _snackbar.Add("Operația a fost creată.", Severity.Success);
                    return id;
                }
            }
            _snackbar.Add("A apărut o eroare...", Severity.Error);
            return default;
        }

        public async Task<Unit> OperationDelete(Guid id)
        {
            var result = await _httpClient.DeleteAsync($"api/Operation/delete/{id}");
            if (result.IsSuccessStatusCode)
            {
                _snackbar.Add("Operația a fost ștersă.", Severity.Success);
                return await result.Content.ReadFromJsonAsync<Unit>();
            }
            _snackbar.Add("A apărut o eroare...", Severity.Error);
            return default;
        }

        public async Task<Unit> OperationEdit(OperationDto request)
        {
            var result = await _httpClient.PutAsJsonAsync("api/Operation/edit", request);
            if (result.IsSuccessStatusCode)
            {
                _snackbar.Add("Operația a fost editată.", Severity.Success);
                return await result.Content.ReadFromJsonAsync<Unit>();
            }
            _snackbar.Add("A apărut o eroare...", Severity.Error);
            return default;
        }

        public async Task<OperationDto> GetOperationById(Guid id)
        {
            var result = await _httpClient.GetAsync($"api/Operation/{id}");
            return await result.Content.ReadFromJsonAsync<OperationDto>();
        }
    }
}
