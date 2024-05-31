using System.IdentityModel.Tokens.Jwt;
using BabyMonitorApiBusiness.Abstractions;
using BabyMonitorApiBusiness.Concretes;
using BabyMonitorApiDataAccess;
using BabyMonitorApiDataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.Cookies;
using BabyMonitorApiDataAccess.Repositories.Abstractions;
using BabyMonitorApiDataAccess.Repositories.Concretes;

var builder = WebApplication.CreateBuilder(args);

// Add cors
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
// builder.Services.AddCors(options =>
// {
//     options.AddPolicy(name: MyAllowSpecificOrigins,
//                       policy =>
//                       {
//                           policy.
//                             WithOrigins("*").
//                             AllowCredentials().
//                             AllowAnyMethod().
//                             AllowAnyHeader();
//                       });
// });

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure the database context
var connectionString = builder.Configuration.GetConnectionString("BabyMonitorContext");
builder.Services.AddDbContext<BabyMonitorContext>(options =>
{
    options.UseSqlServer(connectionString);
});

// Dependency injection

// Users
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IRepository<User>, UserRepository>();

// Monitoring Devices
builder.Services.AddTransient<IRepository<MonitoringDevice>, MonitoringDeviceRepository>();
builder.Services.AddTransient<IDeviceService, DeviceService>();

// Baby
builder.Services.AddTransient<IRepository<Baby>, BabyRepository>();
builder.Services.AddTransient<IBabyService, BabyService>();

// Factory Monitoring Devices
builder.Services.AddTransient<IRepository<FactoryMonitoringDevice>, FactoryMonitoringDeviceRepository>();

// Api Keys
builder.Services.AddTransient<IApiKeyService, ApiKeyService>();
builder.Services.AddTransient<IRepository<ApiKey>, BaseRepository<ApiKey>>();

// Livestream
builder.Services.AddTransient<IRepository<Livestream>, BaseRepository<Livestream>>();
builder.Services.AddTransient<ILivestreamService, LivestreamService>();

// Baby State
builder.Services.AddTransient<IRepository<BabyState>, BaseRepository<BabyState>>();

// Add mvc
builder.Services.AddMvc().AddViewOptions(options =>
{
    options.HtmlHelperOptions.ClientValidationEnabled = true;
});

// Add authentication handler
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
        options.SlidingExpiration = true;
        options.AccessDeniedPath = "/Forbidden/";
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStaticFiles();
app.UseCors(MyAllowSpecificOrigins);

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
