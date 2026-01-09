using Microsoft.EntityFrameworkCore;
using ProjectCore.Application;
using ProjectCore.Application.Common.Security;
using ProjectCore.Application.UseCases.Permissions.Scan;
using ProjectCore.Infrastructure;
using ProjectCore.Infrastructure.Permissions;
using ProjectCore.Infrastructure.Persistence;
using ProjectCore.Infrastructure.Security;
using ProjectCore.Presentation.MVC.Authentication.Claims;
using ProjectCore.Presentation.MVC.Authentication.SignIn;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    });

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();
builder.Services.AddScoped<IPermissionScanner, MvcPermissionScanner>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IUserClaimsFactory, UserClaimsFactory>();
builder.Services.AddScoped<ICookieSignInService, CookieSignInService>();

var app = builder.Build();

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
