using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using ProjectCore.Application;
using ProjectCore.Application.Common.Security;
using ProjectCore.Application.UseCases.Permissions.Scan;
using ProjectCore.Application.UseCases.SeedData;
using ProjectCore.Infrastructure;
using ProjectCore.Infrastructure.Permissions;
using ProjectCore.Infrastructure.Persistence;
using ProjectCore.Infrastructure.Security;
using ProjectCore.Presentation.MVC.Authentication.Claims;
using ProjectCore.Presentation.MVC.Authentication.SignIn;

// Load .env file nếu có
Env.Load();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
        options.SlidingExpiration = true;
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.Cookie.SameSite = SameSiteMode.Lax;
    });

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();
builder.Services.AddScoped<IPermissionScanner, MvcPermissionScanner>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IUserClaimsFactory, UserClaimsFactory>();
builder.Services.AddScoped<ICookieSignInService, CookieSignInService>();

var app = builder.Build();

// Seed dữ liệu tự động khi app start lần đầu
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var seedDataHandler = services.GetRequiredService<SeedDataHandler>();
        await seedDataHandler.SeedAsync();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Lỗi khi seed dữ liệu khi khởi động ứng dụng");
        throw;
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
