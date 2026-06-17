using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Savemate.Application.Interfaces.Services;
using Savemate.Application.Services.IService;
using Savemate.Application.Services.IService.IAccountService;
using Savemate.Application.ViewModels;
using Savemate.Domain.Entities;
using Savemate.Domain.Enums;
using Savemate.Infrastructure;

namespace Savemate.Web.Controllers
{
    public class TransactionController : Controller
    {
        private readonly ITransactionService _transactionService;
        private readonly IAccountService _accountService;
        private readonly ICategoryService _categoryService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<TransactionController> _logger;

        public TransactionController(
            ITransactionService transactionService,
            IAccountService accountService,
            ICategoryService categoryService,
            UserManager<ApplicationUser> userManager,
            ILogger<TransactionController> logger)
        {
            _transactionService = transactionService;
            _accountService = accountService;
            _categoryService = categoryService;
            _userManager = userManager;
            _logger = logger;
        }

        // INDEX
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
                FromAccountId = t.FromAccountId,
                ToAccountId = t.ToAccountId,
                FromAccountName = t.FromAccount?.Name,
                ToAccountName = t.ToAccount?.Name,
                IsReversed = t.IsReversed,
                IsReversalEntry = t.IsReversalEntry,
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
            var userId = _userManager.GetUserId(User);

            var validationErrors = vm.ValidateTransaction();

            foreach(var error in validationErrors)
                ModelState.AddModelError(string.Empty, error);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Transaction creation failed validation for user {UserId}. Errors:{Errors}", userId, 
                    string.Join(",", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
               
                vm = await BuildTransactionViewModel(vm);
                
                return View(vm);
            }

            var transaction = new Transaction
            {
                UserId = userId,
                Amount = vm.Amount,
                Date =  DateTime.UtcNow,
                Note = vm.Note,
                Type = vm.Type,
                FromAccountId = vm.FromAccountId,
                ToAccountId = vm.ToAccountId
            };

            var result = await _transactionService.CreateTransactionAsync(transaction);
            if (!result.IsSuccess)
            {
                _logger.LogWarning("Transaction creation rejected for user {UserId}. Reason:{Reason}.Amount:{Amount}", userId, result.Message, vm.Amount);
                ModelState.AddModelError("", result.Message);

                vm = await BuildTransactionViewModel(vm);

                return View(vm);
            }

            _logger.LogInformation(
           "Transaction created. UserId: {UserId}, Type: {Type}, Amount: {Amount}, TransactionId: {TransactionId}",
           userId, vm.Type, vm.Amount, result.Result?.Id);

            TempData["Success"] = "Transaction created successfully.";
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> GetAccountBalance(int accountId)
        {

            var userId = _userManager.GetUserId(User);

            var account = await _accountService.GetAccountById(accountId, userId);

            if (account == null)
                return NotFound();
            return Json(new { balance = account.InitialBalance });
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
                ToAccountId = transaction.ToAccountId
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

            await _transactionService.UpdateTransactionAsync(existing);

            TempData["Success"] = "Transaction updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        // DELETE GET
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = _userManager.GetUserId(User);
            var transaction = await _transactionService.GetTransactionAsync(id, userId);
            if (transaction == null) return NotFound();

            return View(transaction);
        }

       // DELETE POST
       [HttpPost, ActionName("Delete")]
         [ValidateAntiForgeryToken]

        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = _userManager.GetUserId(User);
            await _transactionService.DeleteTransactionAsync(id, userId);

            TempData["Success"] = "Transaction deleted successfully.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult>Details(int id)
        {

            var userId = _userManager.GetUserId(User);
            var t = await _transactionService.GetTransactionAsync(id, userId);
            if (t == null)
                return NotFound();

            var viewModelList = new TransactionViewModel
            {
                Id = t.Id,
                Type = t.Type,
                Amount = t.Amount,
                Date = t.Date,
                Note = t.Note,
                FromAccountId = t.FromAccountId,
                ToAccountId = t.ToAccountId,
                FromAccountName = t.FromAccount?.Name,
                ToAccountName = t.ToAccount?.Name,
                IsReversed = t.IsReversed,
                IsReversalEntry = t.IsReversalEntry,
            };



            return View(viewModelList);
             
        }


        

        [HttpPost, ActionName("Reverse")]

        [ValidateAntiForgeryToken]

        public async Task<IActionResult> ReverseConfirmed(int id)
        {
            var userId = _userManager.GetUserId(User);

            var result = await _transactionService.ReverseTransactionAsync(id, userId);

            if (!result.IsSuccess)
            {
                TempData["Error"] = result.Message;
            }
            else
            {
                TempData["Success"] = "Transaction reversed successfully.";
            }

            return RedirectToAction(nameof(Index));
        }

        

        private async Task<TransactionViewModel> BuildTransactionViewModel(TransactionViewModel? existing = null)
        {
            var userId = _userManager.GetUserId(User);
            var accounts = await _accountService.GetAccountsByUserAsync(userId);

            var categoriesObj = await _categoryService.GetCategoriesByUserAsync(userId);
            var categories = categoriesObj as IEnumerable<Category> ?? categoriesObj;

            return new TransactionViewModel
            {
                Id = existing?.Id ?? 0,
                Type = existing?.Type ?? TransactionTypeEnum.Expense,
                Amount = existing?.Amount ?? 0,
                Date = existing?.Date ?? DateTime.Now,
                Note = existing?.Note,
                FromAccountId = existing?.FromAccountId,
                ToAccountId = existing?.ToAccountId,
                IsReversed = existing?.IsReversed ?? false,
                IsReversalEntry = existing?.IsReversalEntry ?? false,

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
