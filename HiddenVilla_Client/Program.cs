using HiddenVilla_Client;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Blazored.LocalStorage;
using HiddenVilla_Client.Service.IService;
using HiddenVilla_Client.Service;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.Configuration.GetValue<string>("BaseAPIUrl")) });
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<IHotelRoomService, HotelRoomService>();
builder.Services.AddScoped<IRoomOrderDetailsService, RoomOrderDetailsService>();
await builder.Build().RunAsync();
