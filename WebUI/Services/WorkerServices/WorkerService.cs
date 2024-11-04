using Application.DTOs;
using Application.Interfaces.Pagination;
using MediatR;
using MudBlazor;
using System.Net.Http.Json;

namespace WebUI.Services.WorkerServices
{
    public class WorkerService : IWorkerService
    {
        private readonly HttpClient _httpClient;
        private readonly ISnackbar _snackbar;

        public WorkerService(HttpClient httpClient, ISnackbar snackbar)
        {
            _httpClient = httpClient;
            _snackbar = snackbar;
        }

        public async Task<PaginationResult<WorkerDto>> GetWorkers(PaginationParameter paginationParameter)
        {
            var result = await _httpClient.PostAsJsonAsync("api/Worker/all", paginationParameter);
            if (!result.IsSuccessStatusCode) return default;

            return await result.Content.ReadFromJsonAsync<PaginationResult<WorkerDto>>();
        }

        public async Task<Guid> WorkerCreate(WorkerDto request)
        {
            var result = await _httpClient.PostAsJsonAsync("api/Worker/create", request);
            if (result.IsSuccessStatusCode)
            {
                var id = await result.Content.ReadFromJsonAsync<Guid>();
                if (id == Guid.Empty)
                {
                    _snackbar.Add($"Lucrător cu e-mailul {request.Email} există", Severity.Error);
                    return default;
                }
                else
                {
                    _snackbar.Add("Lucrătorul a fost creat.", Severity.Success);
                    return id;
                }
            }
            _snackbar.Add("A apărut o eroare...", Severity.Error);
            return default;
        }

        public async Task<Unit> WorkerDelete(Guid id)
        {
            var result = await _httpClient.DeleteAsync($"api/Worker/delete/{id}");
            if (result.IsSuccessStatusCode)
            {
                _snackbar.Add("Lucrătorul a fost șters.", Severity.Success);
                return await result.Content.ReadFromJsonAsync<Unit>();
            }
            _snackbar.Add("A apărut o eroare...", Severity.Error);
            return default;
        }

        public async Task<Unit> WorkerEdit(WorkerDto request)
        {
            var result = await _httpClient.PutAsJsonAsync("api/Worker/edit", request);
            if (result.IsSuccessStatusCode)
            {
                _snackbar.Add("Lucrătorul a fost editat.", Severity.Success);
                return await result.Content.ReadFromJsonAsync<Unit>();
            }
            _snackbar.Add("A apărut o eroare...", Severity.Error);
            return default;
        }

        public async Task<WorkerDto> GetWorkerById(Guid id)
        {
            var result = await _httpClient.GetAsync($"api/Worker/{id}");
            return await result.Content.ReadFromJsonAsync<WorkerDto>();
        }

        public async Task<WorkerDto> GetWorkerByEmail(string email)
        {
            var result = await _httpClient.GetAsync($"api/Worker/email={email}");
            return await result.Content.ReadFromJsonAsync<WorkerDto>();
        }

        public async Task<List<WorkerHoursDto>> GetWorkerHours(int month, int year)
        {
            var result = await _httpClient.GetAsync($"api/Worker/workerhours/{month}/{year}");
            return await result.Content.ReadFromJsonAsync<List<WorkerHoursDto>>();
        }

        public async Task<List<double>> GetPercentages()
        {
            var result = await _httpClient.GetAsync($"api/Worker/percentages");
            return await result.Content.ReadFromJsonAsync<List<double>>();
        }

        public async Task<Guid> PercentCreate(double percent)
        {
            var result = await _httpClient.PostAsJsonAsync("api/Worker/PercentCreate", percent);
            if (result.IsSuccessStatusCode)
            {
                var id = await result.Content.ReadFromJsonAsync<Guid>();
                if (id == Guid.Empty)
                {
                    _snackbar.Add($"Procent-ul de gardă {percent} există", Severity.Error);
                    return default;
                }
                else
                {
                    _snackbar.Add("Procent-ul de gardă a fost creat.", Severity.Success);
                    return id;
                }
            }
            _snackbar.Add("A apărut o eroare...", Severity.Error);
            return default;
        }

        public async Task<Unit> PercentDelete(Guid id)
        {
            var result = await _httpClient.DeleteAsync($"api/Worker/PercentDelete/{id}");
            if (result.IsSuccessStatusCode)
            {
                _snackbar.Add("Procent-ul de gardă a fost șters.", Severity.Success);
                return await result.Content.ReadFromJsonAsync<Unit>();
            }
            _snackbar.Add("A apărut o eroare...", Severity.Error);
            return default;
        }

        public async Task<List<PercentDto>> GetPercentList()
        {
            var result = await _httpClient.GetAsync($"api/Worker/percentList");
            return await result.Content.ReadFromJsonAsync<List<PercentDto>>();
        }
    }
}
