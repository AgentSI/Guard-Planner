using Application.DTOs;
using MediatR;
using MudBlazor;
using System.Net.Http.Json;

namespace WebUI.Services.ReceiptServices
{
    public class ReceiptService : IReceiptService
    {
        private readonly HttpClient _httpClient;
        private readonly ISnackbar _snackbar;

        public ReceiptService(HttpClient httpClient, ISnackbar snackbar)
        {
            _httpClient = httpClient;
            _snackbar = snackbar;
        }

        public async Task<Guid> ReceiptCreate(ReceiptDto request)
        {
            var result = await _httpClient.PostAsJsonAsync("api/Receipt/create", request);
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
                    _snackbar.Add("Bonul a fost creat.", Severity.Success);
                    return id;
                }
            }
            _snackbar.Add("A apărut o eroare...", Severity.Error);
            return default;
        }

        public async Task<Unit> ReceiptDelete(Guid id)
        {
            var result = await _httpClient.DeleteAsync($"api/Receipt/delete/{id}");
            if (result.IsSuccessStatusCode)
            {
                _snackbar.Add("Bonul a fost șters.", Severity.Success);
                return await result.Content.ReadFromJsonAsync<Unit>();
            }
            _snackbar.Add("A apărut o eroare...", Severity.Error);
            return default;
        }

        public async Task<Unit> ReceiptEdit(ReceiptDto request)
        {
            var result = await _httpClient.PutAsJsonAsync("api/Receipt/edit", request);
            if (result.IsSuccessStatusCode)
            {
                _snackbar.Add("Bonul a fost editat.", Severity.Success);
                return await result.Content.ReadFromJsonAsync<Unit>();
            }
            _snackbar.Add("A apărut o eroare...", Severity.Error);
            return default;
        }

        public async Task<ReceiptDto> GetReceiptById(Guid id)
        {
            var result = await _httpClient.GetAsync($"api/Receipt/{id}");
            return await result.Content.ReadFromJsonAsync<ReceiptDto>();
        }
    }
}
