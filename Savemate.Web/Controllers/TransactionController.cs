using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Savemate.Application.Interfaces.Services;
using Savemate.Application.Services;
using Savemate.Application.Services.IService;
using Savemate.Application.Services.IService.IAccountService;
using Savemate.Application.ViewModels;
using Savemate.Domain.Entities;
using Savemate.Domain.Enums;
using Savemate.Infrastructure;
 
namespace Savemate.Web.Controllers
{
    //  [Authorize]

    public class TransactionController : Controller
    {
        private readonly ITransactionService _transactionService;
        private readonly IAccountService _accountService;
        private readonly ICategoryService _categoryService;
        private readonly Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> _userManager;

        public TransactionController(
            ITransactionService transactionService,
            IAccountService accountService,
            ICategoryService categoryService,
            Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager)
        {
            _transactionService = transactionService;
            _accountService = accountService;
            _categoryService = categoryService;
            _userManager = userManager;
        }

        // INDEX - list user's transactions
        
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var transactions = await _transactionService.GetTransactionsByUserAsync(userId);

            var viewModelList = transactions.Select(t => new TransactionViewModel
            {
                Id = t.Id,
                Type = t.Type,
                Amount = t.Amount,
                Date = t.Date,
                Note = t.Note,
              //  Category = t.Category,
                FromAccountId = t.FromAccountId,
                ToAccountId = t.ToAccountId,
               // CategoryName = t.Category,          // If you have navigation property
                FromAccountName = t.FromAccount?.Name,
                ToAccountName = t.ToAccount?.Name
            }).ToList();

            return View(viewModelList);
        }


        // CREATE GET
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var vm = await BuildTransactionViewModel();
            return View(vm);
        }

        // CREATE POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TransactionViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm = await BuildTransactionViewModel(vm);
                return View(vm);
            }

            var userId = _userManager.GetUserId(User);

            var transaction = new Transaction
            {
                UserId = userId,
                Amount = vm.Amount,
                Date = vm.Date,
                Note = vm.Note,
                Type = vm.Type,
                FromAccountId = vm.FromAccountId,
                ToAccountId = vm.ToAccountId,
               
            };

            var created = await _transactionService.CreateTransactionAsync(transaction);

            TempData["Success"] = "Transaction created successfully.";
            return RedirectToAction(nameof(Index));
        }

        // EDIT GET
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var userId = _userManager.GetUserId(User);
            var transaction = await _transactionService.GetTransactionAsync(id, userId);
            if (transaction == null) return NotFound();

            var vm = new TransactionViewModel
            {
                Id = transaction.Id,
                Type = transaction.Type,
                Amount = transaction.Amount,
                Date = transaction.Date,
                Note = transaction.Note,
                FromAccountId = transaction.FromAccountId,
                ToAccountId = transaction.ToAccountId,
               

            };

            vm = await BuildTransactionViewModel(vm);
            return View(vm);
        }

        // EDIT POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TransactionViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm = await BuildTransactionViewModel(vm);
                return View(vm);
            }

            var userId = _userManager.GetUserId(User);
            var existing = await _transactionService.GetTransactionAsync(vm.Id, userId);
            if (existing == null) return NotFound();

            existing.Amount = vm.Amount;
            existing.Date = vm.Date;
            existing.Note = vm.Note;
            existing.Type = vm.Type;
            existing.FromAccountId = vm.FromAccountId;
            existing.ToAccountId = vm.ToAccountId;
            

           
            var updated = await _transactionService.UpdateTransactionAsync(existing);

            TempData["Success"] = "Transaction updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        // DELETE GET (confirm)
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = _userManager.GetUserId(User);
            var transaction = await _transactionService.GetTransactionAsync(id, userId);
            if (transaction == null) return NotFound();

            return View(transaction);
        }

        // DELETE POST (confirmed)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = _userManager.GetUserId(User);
            await _transactionService.DeleteTransactionAsync(id, userId);

            TempData["Success"] = "Transaction deleted successfully.";
            return RedirectToAction(nameof(Index));
        }

        // ----------------------
        // Private helper: BuildTransactionViewModel
        // ----------------------
        private async Task<TransactionViewModel> BuildTransactionViewModel(TransactionViewModel? existing = null)
        {
            var userId = _userManager.GetUserId(User);

            // You said you chose casting (Option 2). Keep that here.
            // If your account service returns a strongly-typed IEnumerable<Account> later, remove the cast.
            var accounts = (IEnumerable<Account>)await _accountService.GetAllAccounts();
            var categoriesObj = await _categoryService.GetCategoriesByUserAsync(userId);
            var categories = categoriesObj as IEnumerable<Category> ?? (IEnumerable<Category>)categoriesObj;

            return new TransactionViewModel
            {
                Id = existing?.Id ?? 0,
                Type = existing?.Type ?? TransactionTypeEnum.Expense,
                Amount = existing?.Amount ?? 0,
                Date = existing?.Date ?? DateTime.Now,
                Note = existing?.Note,
                FromAccountId = existing?.FromAccountId,
                ToAccountId = existing?.ToAccountId,
              //  Category = existing.Category,

                Accounts = accounts.Select(a => new SelectListItem
                {
                    Value = a.Id.ToString(),
                    Text = a.Name
                }),

                Categories = Enum.GetValues(typeof(CategoryTypeEnum))
                 .Cast<CategoryTypeEnum>()
                 .Select(c => new SelectListItem
                 {
                     Value = ((int)c).ToString(),
                     Text = c.ToString()
                 })
            };
        }
    }
    }
 
