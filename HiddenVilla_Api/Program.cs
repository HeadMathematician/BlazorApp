using Business.Repository;
using Business.Repository.IRepository;
using DataAccess.Data;
using HiddenVilla_Api.Helper;
using HiddenVilla_Server.Service.IService;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using Microsoft.Extensions.Configuration;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

var configuration = new ConfigurationBuilder().SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json")
    .Build();

// Add services to the container.

builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlServer("Server=DESKTOP-MATE\\MSSQLSERVER01; Database=HiddenVilla; Trusted_Connection=True; MultipleActiveResultSets=true; Encrypt=false;"));


builder.Services.AddIdentity<ApplicationUser, IdentityRole>().
    AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders().AddDefaultUI();


var appSettingsSection = configuration.GetSection("APISettings");
builder.Services.Configure<APISettings>(appSettingsSection);

var apiSettings = appSettingsSection.Get<APISettings>();
var key = Encoding.ASCII.GetBytes(apiSettings.SecretKey);

builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateAudience = true,
        ValidateIssuer = true,
        ValidAudience = apiSettings.ValidAudience,
        ValidIssuer = apiSettings.ValidIssuer,
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddControllers();
builder.Services.AddScoped<IRoomOrderDetailsRepository, RoomOrderDetailsRepository>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IHotelRoomRepository, HotelRoomRepository>();

builder.Services.AddCors(o => o.AddPolicy("HiddenVilla", builder =>
{
    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
}));


builder.Services.AddRouting(option => option.LowercaseUrls = true);
builder.Services.AddControllers().AddJsonOptions(opt => opt.JsonSerializerOptions.PropertyNamingPolicy = null)
    .AddNewtonsoftJson(opt =>
    {
        opt.SerializerSettings.ContractResolver = new DefaultContractResolver();
        opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    });

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "HiddenVilla_Api", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please Bearer and then token in the field",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                   {
                     new OpenApiSecurityScheme
                     {
                       Reference = new OpenApiReference
                       {
                         Type = ReferenceType.SecurityScheme,
                         Id = "Bearer"
                       }
                      },
                      new string[] { }
                    }
                });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("HiddenVilla");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
