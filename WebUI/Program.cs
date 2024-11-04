using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using Application.DTOs;
using Microsoft.AspNetCore.Components.Authorization;
using WebUI.Services.UserAccountServices;
using WebUI.Pages.Shared;
using WebUI.Services.TokenServices;
using WebUI.Services.WorkerServices;
using WebUI.Services.GuardServices;
using WebUI.Services.OperationServices;
using WebUI.Services.ReceiptServices;
using WebUI.Services.InventoryServices;
using WebUI.Services.VacationServices;
using WebUI.Services.InstrumentServices;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddHttpClient("GuardPlanner.ServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));

builder.Services.AddScoped<IMeService, MeService>();
builder.Services.AddScoped<IUserAccountService, UserAccountService>();
builder.Services.AddScoped<IWorkerService, WorkerService>();
builder.Services.AddScoped<IGuardService, GuardService>(); 
builder.Services.AddScoped<IOperationService, OperationService>();
builder.Services.AddScoped<IReceiptService, ReceiptService>();
builder.Services.AddScoped<IInventoryService, InventoryService>();
builder.Services.AddScoped<IInstrumentService, InstrumentService>();
builder.Services.AddScoped<IVacationService, VacationService>();

builder.Services.AddAuthorizationCore();

builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddScoped<CurrentUserService>();
builder.Services.AddSingleton<CurrentUserDto>();

builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("GuardPlanner.ServerAPI"));

builder.Services.AddMudServices();

await builder.Build().RunAsync();
