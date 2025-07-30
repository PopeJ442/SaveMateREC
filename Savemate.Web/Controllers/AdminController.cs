using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Savemate.Domain;
using Savemate.Infrastructure;
using Savemate.Web.ViewModels;



namespace Savemate.Web.Controllers
{
    public class AdminController(UserManager<ApplicationUser> userManager, IPasswordHasher<ApplicationUser> passwordHasher, IPasswordValidator<ApplicationUser> passwordValidator, IUserValidator<ApplicationUser> userValidator) : Controller
    {
         
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IPasswordHasher<ApplicationUser> _passwordHasher = passwordHasher;
        private readonly IUserValidator<ApplicationUser> _userValidator = userValidator;
        private readonly IPasswordValidator<ApplicationUser> _passwordValidator = passwordValidator;
     // [Authorize]
        public IActionResult Index()
        {
            var users = _userManager.Users.ToList();
            return View(users);
        }
    
        public IActionResult Create()
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


        public IActionResult Update()
        {
            return View();
        }

        //public async Task<IActionResult> Delete(string id)
        //{
        //    ApplicationUser user = await _userManager.FindByIdAsync(id);
        //    return View(user);
        //}


        //[HttpPost]
        //public async Task<IActionResult> Create(RegisterViewModel user)
        //{



        //    if (ModelState.IsValid)
        //    {
        //        ApplicationUser appUser = new ApplicationUser
        //        { UserName = user.UserName, 
        //            FirstName = user.UserName,
        //            MiddleName = user.MiddleName,
        //            LastName = user.LastName,
        //            Email = user.Email,
        //            Country = user.CountryCode
        //        };

        //        IdentityResult result = await _userManager.CreateAsync(appUser, user.Password);
        //        if (result.Succeeded)
        //        {
        //            return RedirectToAction("Index");
        //        }
        //        else
        //        {
        //            foreach (IdentityError error in result.Errors)
        //            {
        //                Console.WriteLine(error.Description);
        //                ModelState.AddModelError(string.Empty, error.Description);  

        //            }
        //        }

        //    }
        //    return View(user);
        //}
        [HttpPost]
        public async Task<IActionResult> Create(RegisterViewModel model)
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
                DOB = model.DOB
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
                return RedirectToAction("Index");

            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            model.Countries = CountryHelper.GetAllCountries()
                .Select(c => new SelectListItem { Value = c.CommonName, Text = c.CommonName })
                .ToList();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(string id, string email, string password)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                IdentityResult ValidateEmail = null;
              
                if (!string.IsNullOrEmpty(email)) {


                    ValidateEmail = await _userValidator.ValidateAsync(_userManager, user);
                    if (ValidateEmail.Succeeded)
                    user.Email = email;
                }
              
                else
                    ModelState.AddModelError("", "Email cannot be empty");

                IdentityResult validatePassword = null;
                if (!string.IsNullOrEmpty(password)) {
                   validatePassword = await _passwordValidator.ValidateAsync(_userManager,user,password);
                    if (validatePassword.Succeeded)
                    user.PasswordHash = passwordHasher.HashPassword(user, password);
                }
                else
                    ModelState.AddModelError("", "Password cannot be empty");

                if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password))
                {
                    IdentityResult result = await userManager.UpdateAsync(user);
                    if (result.Succeeded)
                        return RedirectToAction("Index");
                    else
                        Errors(result);
                }
            }
            else
                ModelState.AddModelError("", "User Not Found");
            return View(user);
        }
    

        [HttpGet]
   
        public async Task<IActionResult> Delete(string id) 
        {
            ApplicationUser user =await _userManager.FindByIdAsync(id);

            if (user != null) 
            {
            IdentityResult result =await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                    return RedirectToAction("Index");
                else
                    Errors(result);

            }
            else
            {
                ModelState.AddModelError("","User not found");
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
        private void Errors(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
                ModelState.AddModelError("", error.Description);
        }


    }
}