using Application.DTOs;
using Application.Interfaces.Pagination;
using MediatR;
using MudBlazor;
using System.Net.Http.Json;

namespace WebUI.Services.GuardServices
{
    public class GuardService : IGuardService
    {
        private readonly HttpClient _httpClient;
        private readonly ISnackbar _snackbar;

        public GuardService(HttpClient httpClient, ISnackbar snackbar)
        {
            _httpClient = httpClient;
            _snackbar = snackbar;
        }

        public async Task<PaginationResult<GuardDto>> GetGuards(PaginationParameter paginationParameter)
        {
            var result = await _httpClient.PostAsJsonAsync("api/Guard/all", paginationParameter);
            if (!result.IsSuccessStatusCode) return default;

            return await result.Content.ReadFromJsonAsync<PaginationResult<GuardDto>>();
        }

        public async Task<PaginationResult<GuardDto>> GetGuards(PaginationParameter paginationParameter, Guid workerId)
        {
            var result = await _httpClient.PostAsJsonAsync($"api/Guard/all/{workerId}", paginationParameter);
            if (!result.IsSuccessStatusCode) return default;

            return await result.Content.ReadFromJsonAsync<PaginationResult<GuardDto>>();
        }

        public async Task<Guid> GuardCreate(GuardDto request)
        {
            var result = await _httpClient.PostAsJsonAsync("api/Guard/create", request);
            if (result.IsSuccessStatusCode)
            {
                var id = await result.Content.ReadFromJsonAsync<Guid>();
                if (id == Guid.Empty)
                {
                    _snackbar.Add($"Garda de pe data {request.Date?.ToString("dd.MM.yyyy")} există", Severity.Error);
                    return default;
                }
                else
                {
                    _snackbar.Add("Garda a fost creată.", Severity.Success);
                    return id;
                }
            }
            _snackbar.Add("A apărut o eroare...", Severity.Error);
            return default;
        }

        public async Task<Unit> GuardDelete(Guid id)
        {
            var result = await _httpClient.DeleteAsync($"api/Guard/delete/{id}");
            if (result.IsSuccessStatusCode)
            {
                _snackbar.Add("Garda a fost ștersă.", Severity.Success);
                return await result.Content.ReadFromJsonAsync<Unit>();
            }
            _snackbar.Add("A apărut o eroare...", Severity.Error);
            return default;
        }

        public async Task<Unit> GuardEdit(GuardDto request)
        {
            var result = await _httpClient.PutAsJsonAsync("api/Guard/edit", request);
            if (result.IsSuccessStatusCode)
            {
                _snackbar.Add("Garda a fost editată.", Severity.Success);
                return await result.Content.ReadFromJsonAsync<Unit>();
            }
            _snackbar.Add("A apărut o eroare...", Severity.Error);
            return default;
        }

        public async Task<GuardDto> GetGuardById(Guid id)
        {
            var result = await _httpClient.GetAsync($"api/Guard/{id}");
            return await result.Content.ReadFromJsonAsync<GuardDto>();
        }

        public async Task<List<WorkerDto>> GetWorkers()
        {
            var result = await _httpClient.GetAsync($"api/worker/workers");
            return await result.Content.ReadFromJsonAsync<List<WorkerDto>>();
        }

        public async Task<List<GuardDto>> GenerateGuards(int month, int year)
        {
            var result = await _httpClient.GetAsync($"api/Guard/{month}/{year}");
            return await result.Content.ReadFromJsonAsync<List<GuardDto>>();
        }

        public async Task<Unit> SaveGuards(List<GuardDto> guards)
        {
            var result = await _httpClient.PostAsJsonAsync("api/Guard/save", guards);
            if (result.IsSuccessStatusCode)
            {
                _snackbar.Add("Grafic-ul de gărzi a fost salvat.", Severity.Success);
                return await result.Content.ReadFromJsonAsync<Unit>();
            }
            _snackbar.Add("A apărut o eroare...", Severity.Error);
            return default;
        }

        public async Task<List<GuardDto>> GetGuards(int month, int year)
        {
            var result = await _httpClient.GetAsync($"api/Guard/all/{month}/{year}");
            return await result.Content.ReadFromJsonAsync<List<GuardDto>>();
        }

        public async Task<Unit> SendGuards(List<GuardDto> guards)
        {
            var result = await _httpClient.PostAsJsonAsync("api/Guard/send", guards);
            if (result.IsSuccessStatusCode)
            {
                _snackbar.Add($"Grafic-ul de gărzi pentru a fost trimis lucrătorilor.", Severity.Success);
                return await result.Content.ReadFromJsonAsync<Unit>();
            }
            _snackbar.Add("A apărut o eroare...", Severity.Error);
            return default;
        }

        public async Task<Unit> DeleteGuards(DateTime? date)
        {
            var result = await _httpClient.PostAsJsonAsync("api/Guard/deleteGuards", date);
            if (result.IsSuccessStatusCode)
            {
                _snackbar.Add($"Grafic-ul de gărzi a fost șters.", Severity.Success);
                return await result.Content.ReadFromJsonAsync<Unit>();
            }
            _snackbar.Add("A apărut o eroare...", Severity.Error);
            return default;
        }
    }
}
