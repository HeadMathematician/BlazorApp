using DataAccess.Data;
using HiddenVilla_Server.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using Business.Repository.IRepository;
using System.Collections.Generic;
using Business.Repository;
using HiddenVilla_Server.Service.IService;
using Microsoft.AspNetCore.Identity;
using HiddenVilla_Server.Service;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("ApplicationDbContextConnection") ?? throw new InvalidOperationException("Connection string 'ApplicationDbContextConnection' not found.");

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options => 
options.UseSqlServer("Server=DESKTOP-MATE\\MSSQLSERVER01; Database=HiddenVilla; Trusted_Connection=True; MultipleActiveResultSets=true; Encrypt=false;"));

builder.Services.AddIdentity<IdentityUser, IdentityRole>().
    AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders().AddDefaultUI();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<IHotelRoomRepository, HotelRoomRepository>();
builder.Services.AddScoped<IHotelImagesRepository, HotelImagesRepository>();
builder.Services.AddScoped<IFileUpload, FileUpload>();
builder.Services.AddScoped<IDbInitializer, DbInitializer>();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
    dbInitializer.Initalize();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();

app.UseAuthorization();
app.MapRazorPages();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");



app.Run();
