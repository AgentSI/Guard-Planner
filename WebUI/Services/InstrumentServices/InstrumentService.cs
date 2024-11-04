using Application.DTOs;
using Application.Interfaces.Pagination;
using MediatR;
using MudBlazor;
using System.Net.Http.Json;

namespace WebUI.Services.InstrumentServices
{
    public class InstrumentService : IInstrumentService
    {
        private readonly HttpClient _httpClient;
        private readonly ISnackbar _snackbar;

        public InstrumentService(HttpClient httpClient, ISnackbar snackbar)
        {
            _httpClient = httpClient;
            _snackbar = snackbar;
        }

        public async Task<PaginationResult<InstrumentDto>> GetInstruments(PaginationParameter paginationParameter, Guid guardId)
        {
            var result = await _httpClient.PostAsJsonAsync($"api/Instrument/all/{guardId}", paginationParameter);
            if (!result.IsSuccessStatusCode) return default;

            return await result.Content.ReadFromJsonAsync<PaginationResult<InstrumentDto>>();
        }

        public async Task<Guid> InstrumentCreate(InstrumentDto request)
        {
            var result = await _httpClient.PostAsJsonAsync("api/Instrument/create", request);
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
                    _snackbar.Add("Instrument-ul a fost creat.", Severity.Success);
                    return id;
                }
            }
            _snackbar.Add("A apărut o eroare...", Severity.Error);
            return default;
        }

        public async Task<Unit> InstrumentDelete(Guid id)
        {
            var result = await _httpClient.DeleteAsync($"api/Instrument/delete/{id}");
            if (result.IsSuccessStatusCode)
            {
                _snackbar.Add("Instrument-ul a fost șters.", Severity.Success);
                return await result.Content.ReadFromJsonAsync<Unit>();
            }
            _snackbar.Add("A apărut o eroare...", Severity.Error);
            return default;
        }

        public async Task<Unit> InstrumentEdit(InstrumentDto request)
        {
            var result = await _httpClient.PutAsJsonAsync("api/Instrument/edit", request);
            if (result.IsSuccessStatusCode)
            {
                _snackbar.Add("Instrument-ul a fost editat.", Severity.Success);
                return await result.Content.ReadFromJsonAsync<Unit>();
            }
            _snackbar.Add("A apărut o eroare...", Severity.Error);
            return default;
        }

        public async Task<InstrumentDto> GetInstrumentById(Guid id)
        {
            var result = await _httpClient.GetAsync($"api/Instrument/{id}");
            return await result.Content.ReadFromJsonAsync<InstrumentDto>();
        }
    }
}
