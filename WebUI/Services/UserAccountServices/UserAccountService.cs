using Application.DTOs;
using Application.Interfaces.Pagination;
using MediatR;
using MudBlazor;
using System.Net.Http.Json;

namespace WebUI.Services.UserAccountServices
{
    public class UserAccountService : IUserAccountService
    {
        private readonly HttpClient _httpClient;
        private readonly ISnackbar _snackbar;

        public UserAccountService(HttpClient httpClient, ISnackbar snackbar)
        {
            _httpClient = httpClient;
            _snackbar = snackbar;
        }

        public async Task<PaginationResult<UserDto>> GetUsers(PaginationParameter paginationParameter)
        {
            var result = await _httpClient.PostAsJsonAsync("api/user/all", paginationParameter);
            if (!result.IsSuccessStatusCode) return default;

            return await result.Content.ReadFromJsonAsync<PaginationResult<UserDto>>();
        }

        public async Task<UserCreateResultDto> UserCreate(UserDto request)
        {
            var result = await _httpClient.PostAsJsonAsync("api/user/create", request);
            if (result.IsSuccessStatusCode)
            {
                var response = await result.Content.ReadFromJsonAsync<UserCreateResultDto>();
                if (response != null)
                {
                    if (response.Success)
                    {
                        _snackbar.Add(response.Message, Severity.Success);
                        return response;
                    }
                    else
                    {
                        _snackbar.Add(response.Message, Severity.Error);
                        return response;
                    }
                }
                else
                {
                    _snackbar.Add("A apărut o eroare la procesarea răspunsului...", Severity.Error);
                    return new UserCreateResultDto { Success = false, Message = "Răspuns invalid." };
                }
            }
            _snackbar.Add("A apărut o eroare la crearea utilizatorului...", Severity.Error);
            return new UserCreateResultDto { Success = false, Message = "Eroare de rețea sau server." };
        }

        public async Task<Unit> UserDelete(Guid id)
        {
            var result = await _httpClient.DeleteAsync($"api/user/delete/{id}");
            if (result.IsSuccessStatusCode)
            {
                _snackbar.Add("Utilizatorul a fost șters cu succes.", Severity.Success);
                return await result.Content.ReadFromJsonAsync<Unit>();
            }
            _snackbar.Add("A apărut o eroare...", Severity.Error);
            return default;
        }

        public async Task<Unit> UserEdit(UserDto request)
        {
            var result = await _httpClient.PutAsJsonAsync("api/user/edit", request);
            if (result.IsSuccessStatusCode)
            {
                _snackbar.Add("Utilizatorul a fost editat cu succes.", Severity.Success);
                return await result.Content.ReadFromJsonAsync<Unit>();
            }
            _snackbar.Add("A apărut o eroare...", Severity.Error);
            return default;
        }

        public async Task<UserDto> GetUserById(Guid id)
        {
            var result = await _httpClient.GetAsync($"api/user/{id}");
            return await result.Content.ReadFromJsonAsync<UserDto>();
        }

        public async Task<List<string>> GetRoles()
        {
            var result = await _httpClient.GetAsync("api/user/roles");
            return await result.Content.ReadFromJsonAsync<List<string>>();
        }

        public async Task<AuthResultDto> Login(LoginDto request)
        {
            var result = await _httpClient.PostAsJsonAsync("api/user/login", request);
            return await result.Content.ReadFromJsonAsync<AuthResultDto>();
        }

        public async Task<string> Logout()
        {
            var result = await _httpClient.GetAsync($"api/user/log-out");
            return await result.Content.ReadAsStringAsync();
        }

        public async Task<Unit> UserResetPassword(string email, string password)
        {
            var result = await _httpClient.GetAsync($"api/user/reset-password/{email}&{password}");
            if (result.IsSuccessStatusCode) return await result.Content.ReadFromJsonAsync<Unit>();
            return default;
        }

        public async Task<bool> SendConfirmationCode(string email)
        {
            var result = await _httpClient.GetAsync($"api/user/sendConfirmationCode/{email}");
            if (result.IsSuccessStatusCode) return await result.Content.ReadFromJsonAsync<bool>();
            return default;
        }

        public async Task<bool> VerifyConfirmationCode(string email, string code)
        {
            var result = await _httpClient.GetAsync($"api/user/verifyConfirmationCode/{email}&{code}");
            if (result.IsSuccessStatusCode) return await result.Content.ReadFromJsonAsync<bool>();
            return default;
        }
    }
}
