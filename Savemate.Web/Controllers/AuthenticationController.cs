using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Savemate.Application.Common.Extensions;
using Savemate.Domain.Entities;
using Savemate.Infrastructure;
using Savemate.Web.Models;
using Savemate.Web.ViewModels;
using System.Security.Claims;

namespace Savemate.Web.Controllers
{
    public class AuthenticationController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IPasswordHasher<ApplicationUser> passwordHasher, IPasswordValidator<ApplicationUser> passwordValidator, IUserValidator<ApplicationUser> userValidator) : Controller
    
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly SignInManager<ApplicationUser> _signInManager= signInManager;

         
        private readonly IPasswordHasher<ApplicationUser> _passwordHasher = passwordHasher;
        private readonly IUserValidator<ApplicationUser> _userValidator = userValidator;
        private readonly IPasswordValidator<ApplicationUser> _passwordValidator = passwordValidator;


        public IActionResult Register()
        {
  
         var countries = CountryHelper.GetAllCountries()
        .Select(c => new SelectListItem
        {
            Text = c.CommonName,
            Value = c.CommonName
        }).ToList();

            var viewModel = new RegisterViewModel
            {
                Countries = countries
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Countries = CountryHelper.GetAllCountries()
                    .Select(c => new SelectListItem { Value = c.CommonName, Text = c.CommonName })
                    .ToList();
                return View(model);
            }

            var user = new ApplicationUser
            {
                UserName = model.UserName,
                FirstName = model.FirstName,
                MiddleName = model.MiddleName,
                LastName = model.LastName,
                Email = model.Email,
                Country = model.CountryCode,
                DOB = model.DOB,
              //  TwoFactorEnabled = false
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("index", "account");
                    
            }
         

            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            model.Countries = CountryHelper.GetAllCountries()
                .Select(c => new SelectListItem { Value = c.CommonName, Text = c.CommonName })
                .ToList();

            return View(model);
        }

        public IActionResult Update()
        {
            return View();
        }

        
        [HttpPost]
        public async Task<IActionResult> Update(UpdateUserViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null)
                return NotFound();
             
            user.Email = model.Email;
            user.MiddleName = model.MiddleName;
            user.LastName = model.LastName;

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                foreach (var error in updateResult.Errors)
                    ModelState.AddModelError("", error.Description);

                return View(model);
            }
             
            if (!string.IsNullOrEmpty(model.NewPassword))
            {
                var passwordResult = await _userManager.ChangePasswordAsync(
                    user,
                    model.OldPassword,
                    model.NewPassword
                );

                if (!passwordResult.Succeeded)
                {
                    foreach (var error in passwordResult.Errors)
                        ModelState.AddModelError("", error.Description);

                    return View(model);
                }
            }

            return RedirectToAction("Index");
        }

        private void Errors(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
                ModelState.AddModelError("", error.Description);
        }
        public IActionResult Login(string? returnUrl)
        {
            if (User?.Identity?.IsAuthenticated ?? false)
            {
                 
                return RedirectToAction("Index", "Home");
            }
            Login login = new Login();
            login.ReturnUrl = returnUrl;
            return View(login);
        }
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("login", "Authentication");
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(Login login, string? returnUrl = null)
        {
            returnUrl ??= Url.Content("/Home/index");

            if (!ModelState.IsValid)
                return View(login);

            var appUser = await _userManager.FindByEmailAsync(login.Email);
            if (appUser == null)
            {
                ModelState.AddModelError("", "Invalid login attempt.");
                return View(login);
            }

             
            await _signInManager.SignOutAsync();

            var result = await _signInManager.PasswordSignInAsync(appUser, login.Password, login.RememberMe, false);
            if (result.Succeeded)
            {

                return LocalRedirect(returnUrl);
            }

            if (result.RequiresTwoFactor)
            {
                return RedirectToAction("LoginTwoStep", new { email = login.Email, returnUrl });
            }

            ModelState.AddModelError("", "Invalid login attempt.");
            return View(login);
        }

        [AllowAnonymous]
        public IActionResult GoogleLogin()
        {
            string redirectUrl = Url.Action("GoogleResponse", "Authentication");
            var properties = signInManager.ConfigureExternalAuthenticationProperties("Google", redirectUrl);
            return new ChallengeResult("Google", properties);
        }
        public IActionResult AccessDenied()
        {
            return View();
        }


        [AllowAnonymous]
        public async Task<IActionResult> GoogleResponse()
        {
            ExternalLoginInfo info = await signInManager.GetExternalLoginInfoAsync();
            if (info == null)
                return RedirectToAction(nameof(Login));

            var result = await signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false);
            string[] userInfo = { info.Principal.FindFirst(ClaimTypes.Name).Value, info.Principal.FindFirst(ClaimTypes.Email).Value };
            if (result.Succeeded)
                return View(userInfo);
            else
            {
                ApplicationUser user = new ApplicationUser
                {
                    Email = info.Principal.FindFirst(ClaimTypes.Email).Value,
                    UserName = info.Principal.FindFirst(ClaimTypes.Email).Value,
                    FirstName = info.Principal.FindFirst(ClaimTypes.Name).Value,
                    LastName = info.Principal.FindFirst(ClaimTypes.Name).Value
                };

                IdentityResult identResult = await userManager.CreateAsync(user);
                if (identResult.Succeeded)
                {
                    identResult = await userManager.AddLoginAsync(user, info);
                    if (identResult.Succeeded)
                    {
                        await signInManager.SignInAsync(user, false);
                        return View(userInfo);
                    }
                }
                return AccessDenied();
            }

        }
        [AllowAnonymous]
        public async Task<IActionResult> LoginTwoStep(string email, string returnUrl)
        {
            var user = await userManager.FindByEmailAsync(email);

            var token = await userManager.GenerateTwoFactorTokenAsync(user, "Email");

            EmailHelper emailHelper = new EmailHelper();
            bool emailResponse = emailHelper.SendEmailTwoFactorCode(user.Email, token);

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> LoginTwoStep(TwoFactor twoFactor, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(twoFactor);
            }
             
            var result = await signInManager.TwoFactorSignInAsync("Email", twoFactor.TwoFactorCode, false, false);
            if (result.Succeeded)
            {
                return Redirect(returnUrl ?? "/");
            }
            else
            {
                ModelState.AddModelError("", "Invalid Login Attempt");
                return View(twoFactor);
            }
        }
    }
}
    
