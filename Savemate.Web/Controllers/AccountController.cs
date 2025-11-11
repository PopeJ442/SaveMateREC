using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Savemate.Application.Services.IService.IAccountService;
using Savemate.Domain.Entities;
using Savemate.Infrastructure;
using Savemate.Web.ViewModels; 

namespace Savemate.Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(IAccountService accountService, UserManager<ApplicationUser> userManager)
        {
            _accountService = accountService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var accounts = await _accountService.GetAccountsByUserAsync(userId);

            var viewModel = accounts.Select(a => new AccountViewModel
            {
                Id = a.Id,
                Name = a.Name,
                Type = a.Type,
                InitialBalance = a.InitialBalance
            }).ToList();

            return View(viewModel);
        }

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
                return View(model);

            var userId = _userManager.GetUserId(User);

            var account = new Account
            {
                Name = model.Name,
                Type = model.Type,
                InitialBalance = model.InitialBalance,
                UserId = userId
            };

            await _accountService.AddAccount(account, userId);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var userId = _userManager.GetUserId(User);
            var account = await _accountService.GetAccountById(id, userId);

            if (account == null)
                return NotFound();

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
                return View(model);

            var userId = _userManager.GetUserId(User);
            var existingAccount = await _accountService.GetAccountById(model.Id, userId);
            if (existingAccount == null)
                return NotFound();

            existingAccount.Name = model.Name;
            existingAccount.Type = model.Type;
            existingAccount.InitialBalance = model.InitialBalance;

            await _accountService.UpdateAccount(existingAccount, userId);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            var userId = _userManager.GetUserId(User);
            await _accountService.DeleteAccount(id, userId);
            return RedirectToAction(nameof(Index));
        }
    }
}
