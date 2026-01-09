using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using ProjectCore.Application.UseCases.Users.Commands.Login;
using ProjectCore.Domain.Exceptions;
using ProjectCore.Presentation.MVC.Authentication.Claims;
using ProjectCore.Presentation.MVC.Models;
using System;

public class AccountController : Controller
{
    private readonly LoginUserHandler _loginUserHandler;

    public AccountController(LoginUserHandler loginUserHandler)
    {
        _loginUserHandler = loginUserHandler;
    }

    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(
        LoginViewModel model,
        string? returnUrl,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return View(model);

        try
        {
            var result = await _loginUserHandler.Handle(
                new LoginUserCommand
                {
                    UserNameOrEmail = model.UserNameOrEmail,
                    Password = model.Password
                },
                cancellationToken);

           
            var principal = ClaimsPrincipalBuilder.Build(result);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                new AuthenticationProperties
                {
                    IsPersistent = model.RememberMe,
                    ExpiresUtc = model.RememberMe
                        ? DateTimeOffset.UtcNow.AddDays(30)
                        : DateTimeOffset.UtcNow.AddHours(1)
                });

            if (!string.IsNullOrWhiteSpace(returnUrl) &&
                Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Home");
        }
        catch (InvalidLoginException)
        {
            ModelState.AddModelError("", "Invalid username or password.");
            return View(model);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(
            CookieAuthenticationDefaults.AuthenticationScheme);

        return RedirectToAction("Index", "Home");
    }
}
