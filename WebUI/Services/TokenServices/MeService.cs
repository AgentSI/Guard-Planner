using Application.DTOs;
using System.Net.Http.Json;

namespace WebUI.Services.TokenServices
{
    public class MeService : IMeService
    {
        private readonly HttpClient _httpClient;

        public MeService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ClaimsDto> Me(string jwt)
        {
            var result = await _httpClient.GetAsync("api/auth/tokenme");

            try
            {
                return await result.Content.ReadFromJsonAsync<ClaimsDto>();
            }
            catch (Exception e)
            {
                Console.WriteLine("Me Exception: ", e);
            }

            return null;
        }
    }
}
