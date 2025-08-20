using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Savemate.Application.Services;
using Savemate.Application.Services.IService;
using Savemate.Domain.Entities;
using Savemate.Infrastructure;
using Savemate.Web.ViewModels;


namespace Savemate.Web.Controllers
{
    public class AccountController(IAccountService accountService, UserManager<ApplicationUser> userManager) : Controller
    {
        private readonly IAccountService _accountService = accountService;
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var accounts = await _accountService.GetAllAccount();

          

            var viewModel = accounts.Select(a => new AccountViewModel {
            
            Id = a.Id,
            Name = a.Name,
            Type = a.Type,
            InitialBalance = a.InitialBalance,
            
            
            }).ToList();



            return View(viewModel);
        }
        [Authorize]
        [HttpGet]
        public IActionResult CreateAccount()
        {
            return View(new AccountViewModel());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAccount(AccountViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);

            }
            var userId = _userManager.GetUserId(User);

            var account = new Account
            {
                Name = model.Name,
                Type = model.Type,
                InitialBalance = model.InitialBalance,
                 UserId = userId,

            };
            await _accountService.AddAccount(account);
            return RedirectToAction(nameof(Index));

        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var account = await _accountService.GetAccountById(id);
            if (account == null)
            {
                return NotFound();
            }
            var model = new AccountViewModel
            {
                Id = account.Id,
                Name = account.Name,
                Type = account.Type,
                InitialBalance = account.InitialBalance
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAccount(AccountViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var account = await _accountService.GetAccountById(model.Id);
            if (account == null)
            {
                return NotFound();
            }
             account.Name = model.Name;
            account.Type = model.Type;
            account.InitialBalance = model.InitialBalance;

            await _accountService.UpdateAccount(account);
            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> DeleteAccount(int id)
        {
            var account = await _accountService.GetAccountById(id);
            if (account == null)
            {
                return NotFound();
            }

            await _accountService.DeleteAccount(account);
            return RedirectToAction(nameof(Index));
        }
    }




}
