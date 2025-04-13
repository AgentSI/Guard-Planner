using Application.DTOs;
using MediatR;
using MudBlazor;
using System.Net.Http.Json;

namespace WebUI.Services.HourServices
{
    public class HourService(HttpClient httpClient, ISnackbar snackbar) : IHourService
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly ISnackbar _snackbar = snackbar;

        public async Task<Guid> HourCreate(HourDto hour)
        {
            var result = await _httpClient.PostAsJsonAsync("api/Hour", hour);
            if (result.IsSuccessStatusCode)
            {
                var id = await result.Content.ReadFromJsonAsync<Guid>();
                if (id == Guid.Empty)
                {
                    _snackbar.Add($"Ora de lucru există", Severity.Error);
                    return default;
                }
                else
                {
                    _snackbar.Add("Ora de lucru a fost creată.", Severity.Success);
                    return id;
                }
            }
            _snackbar.Add("A apărut o eroare...", Severity.Error);
            return default;
        }

        public async Task<Unit> HourDelete(Guid id)
        {
            var result = await _httpClient.DeleteAsync($"api/Hour/{id}");
            if (result.IsSuccessStatusCode)
            {
                _snackbar.Add("Ora de lucru a fost ștearsă.", Severity.Success);
                return await result.Content.ReadFromJsonAsync<Unit>();
            }
            _snackbar.Add("A apărut o eroare...", Severity.Error);
            return default;
        }

        public async Task<List<HourDto>> GetHourList()
        {
            var result = await _httpClient.GetAsync($"api/Hour");
            return await result.Content.ReadFromJsonAsync<List<HourDto>>();
        }
    }
}
