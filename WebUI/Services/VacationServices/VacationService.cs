using Application.DTOs;
using Application.Interfaces.Pagination;
using MediatR;
using MudBlazor;
using System.Net.Http.Json;

namespace WebUI.Services.VacationServices
{
    public class VacationService : IVacationService
    {
        private readonly HttpClient _httpClient;
        private readonly ISnackbar _snackbar;

        public VacationService(HttpClient httpClient, ISnackbar snackbar)
        {
            _httpClient = httpClient;
            _snackbar = snackbar;
        }

        public async Task<PaginationResult<VacationDto>> GetVacations(PaginationParameter paginationParameter)
        {
            var result = await _httpClient.PostAsJsonAsync("api/Vacation/all", paginationParameter);
            if (!result.IsSuccessStatusCode) return default;

            return await result.Content.ReadFromJsonAsync<PaginationResult<VacationDto>>();
        }

        public async Task<PaginationResult<VacationDto>> GetVacations(PaginationParameter paginationParameter, Guid workerId)
        {
            var result = await _httpClient.PostAsJsonAsync($"api/Vacation/all/{workerId}", paginationParameter);
            if (!result.IsSuccessStatusCode) return default;

            return await result.Content.ReadFromJsonAsync<PaginationResult<VacationDto>>();
        }

        public async Task<VacationCreateResultDto> VacationCreate(VacationDto request)
        {
            var result = await _httpClient.PostAsJsonAsync("api/Vacation/create", request);
            if (result.IsSuccessStatusCode)
            {
                var response = await result.Content.ReadFromJsonAsync<VacationCreateResultDto>();
                if (response != null)
                {
                    if (response.Success)
                    {
                        _snackbar.Add(response.ErrorMessage, Severity.Success);
                        return response;
                    }
                    else
                    {
                        _snackbar.Add(response.ErrorMessage, Severity.Error);
                        return response;
                    }
                }
                else
                {
                    _snackbar.Add("A apărut o eroare la procesarea răspunsului...", Severity.Error);
                    return new VacationCreateResultDto { Success = false };
                }
            }
            _snackbar.Add("A apărut o eroare...", Severity.Error);
            return new VacationCreateResultDto { Success = false };
        }

        public async Task<Unit> VacationDelete(Guid id)
        {
            var result = await _httpClient.DeleteAsync($"api/Vacation/delete/{id}");
            if (result.IsSuccessStatusCode)
            {
                _snackbar.Add("Concedi-ul a fost șters.", Severity.Success);
                return await result.Content.ReadFromJsonAsync<Unit>();
            }
            _snackbar.Add("A apărut o eroare...", Severity.Error);
            return default;
        }

        public async Task<Unit> VacationEdit(VacationDto request)
        {
            var result = await _httpClient.PutAsJsonAsync("api/Vacation/edit", request);
            if (result.IsSuccessStatusCode)
            {
                _snackbar.Add("Concedi-ul a fost editat.", Severity.Success);
                return await result.Content.ReadFromJsonAsync<Unit>();
            }
            _snackbar.Add("A apărut o eroare...", Severity.Error);
            return default;
        }

        public async Task<VacationDto> GetVacationById(Guid id)
        {
            var result = await _httpClient.GetAsync($"api/Vacation/{id}");
            return await result.Content.ReadFromJsonAsync<VacationDto>();
        }

        public async Task<List<VacationDto>> GetVacationsForMonth(int month, int year)
        {
            var result = await _httpClient.GetAsync($"api/Vacation/{month}/{year}");
            return await result.Content.ReadFromJsonAsync<List<VacationDto>>();
        }
    }
}
