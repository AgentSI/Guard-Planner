using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Net.Http.Headers;
using System.Security.Claims;
using WebUI.Services.TokenServices;

namespace WebUI.Services.UserAccountServices
{
    public class CustomAuthStateProvider(HttpClient httpClient, IJSRuntime jSRuntime, IMeService meService) : AuthenticationStateProvider
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly IJSRuntime _jsRuntime = jSRuntime;
        private readonly IMeService _meService = meService;

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            string authToken = await _jsRuntime.InvokeAsync<string>("getCookie", "token");

            var identity = new ClaimsIdentity();
            var claims = new List<Claim>();

            _httpClient.DefaultRequestHeaders.Authorization = null;

            if (!string.IsNullOrEmpty(authToken))
            {
                try
                {
                    var claimDto = await _meService.Me(authToken);

                    claims.Add(new Claim("userId", claimDto.UserId));
                    claims.Add(new Claim("role", claimDto.Role));
                    claims.Add(new Claim("email", claimDto.Email));

                    identity = new ClaimsIdentity(claims, "jwt");

                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken.Replace("\"", ""));
                }
                catch (Exception e)
                {
                    identity = new ClaimsIdentity();
                    Console.WriteLine(e.Message);
                }
            }

            var user = new ClaimsPrincipal(identity);
            var state = new AuthenticationState(user);

            NotifyAuthenticationStateChanged(Task.FromResult(state));

            return state;
        }

        public async Task Logout()
        {
            await _jsRuntime.InvokeVoidAsync("removeJwtFromCookie", "token");
        }
    }
}
