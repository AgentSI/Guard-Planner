using Application.DTOs;
using System.Net.Http.Json;

namespace WebUI.Services.TokenServices
{
    public class MeService(HttpClient httpClient) : IMeService
    {
        private readonly HttpClient _httpClient = httpClient;

        public async Task<ClaimsDto> Me(string jwt)
        {
            var result = await _httpClient.GetAsync("api/auth/tokenme");

            try
            {
                return await result.Content.ReadFromJsonAsync<ClaimsDto>();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Me Exception: {e.Message}");
            }

            return null;
        }
    }
}
