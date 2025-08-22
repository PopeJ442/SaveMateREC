using Microsoft.AspNetCore.Mvc;
using Savemate.Application.Interface.IServices;
using Savemate.Application.Services.IService;
using Savemate.Application.ViewModels;
using Savemate.Domain.Entities;

namespace Savemate.Web.Controllers
{
    public class TransactionController : Controller
    {
        private readonly ITransactionService _transactionService;
        private readonly IAccountService _accountService;
        private readonly ICategoryService _categoryService;

        public TransactionController(
            ITransactionService transactionService,
            IAccountService accountService,
            ICategoryService categoryService)
        {
            _transactionService = transactionService;
            _accountService = accountService;
            _categoryService = categoryService;
        }

        // GET: Transaction
        public async Task<IActionResult> Index()
        {
            var userId = User.Identity?.Name; // or however you're storing userId
            var transactions = await _transactionService.ListTransactionsByUserAsync(userId!);

            var model = transactions.Select(t => new TransactionViewModel
            {
                Id = t.Id,
                Amount = t.Amount,
                Type = t.Type,
                Date = t.Date,
                Note = t.Note,
                FromAccountId = t.FromAccountId,
                ToAccountId = t.ToAccountId,
                CategoryId = t.CategoryId
            }).ToList();

            return View(model);
        }

        // GET: Transaction/Create
        public async Task<IActionResult> Create()
        {
            var model = new TransactionViewModel
            {
                Accounts = await _accountService.ListAccountsByUserAsync(User.Identity!.Name!),
                Categories = await _categoryService.ListCategoriesByUserAsync(User.Identity!.Name!)
            };
            return View(model);
        }

        // POST: Transaction/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TransactionViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.Accounts = await _accountService.ListAccountsByUserAsync(User.Identity!.Name!);
                vm.Categories = await _categoryService.ListCategoriesByUserAsync(User.Identity!.Name!);
                return View(vm);
            }

            var entity = new Transaction
            {
                Amount = vm.Amount,
                Type = vm.Type,
                Date = vm.Date,
                Note = vm.Note,
                FromAccountId = vm.FromAccountId,
                ToAccountId = vm.ToAccountId,
                CategoryId = vm.CategoryId,
                UserId = Guid.Parse(User.Identity!.Name!) // assuming Guid stored in claims
            };

            await _transactionService.AddTransactionAsync(entity, User.Identity!.Name!);

            return RedirectToAction(nameof(Index));
        }

        // GET: Transaction/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var entity = await _transactionService.GetTransactionByIdAsync(id, User.Identity!.Name!);
            if (entity == null) return NotFound();

            var vm = new TransactionViewModel
            {
                Id = entity.Id,
                Amount = entity.Amount,
                Type = entity.Type,
                Date = entity.Date,
                Note = entity.Note,
                FromAccountId = entity.FromAccountId,
                ToAccountId = entity.ToAccountId,
                CategoryId = entity.CategoryId,
                Accounts = await _accountService.ListAccountsByUserAsync(User.Identity!.Name!),
                Categories = await _categoryService.ListCategoriesByUserAsync(User.Identity!.Name!)
            };

            return View(vm);
        }

        // POST: Transaction/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TransactionViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.Accounts = await _accountService.ListAccountsByUserAsync(User.Identity!.Name!);
                vm.Categories = await _categoryService.ListCategoriesByUserAsync(User.Identity!.Name!);
                return View(vm);
            }

            var entity = new Transaction
            {
                Id = vm.Id,
                Amount = vm.Amount,
                Type = vm.Type,
                Date = vm.Date,
                Note = vm.Note,
                FromAccountId = vm.FromAccountId,
                ToAccountId = vm.ToAccountId,
                CategoryId = vm.CategoryId,
                UserId = Guid.Parse(User.Identity!.Name!)
            };

            await _transactionService.UpdateTransactionAsync(entity, User.Identity!.Name!);

            return RedirectToAction(nameof(Index));
        }

        // GET: Transaction/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _transactionService.GetTransactionByIdAsync(id, User.Identity!.Name!);
            if (entity == null) return NotFound();

            return View(entity); // you can map to VM if you want
        }

        // POST: Transaction/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _transactionService.DeleteTransactionAsync(id, User.Identity!.Name!);
            return RedirectToAction(nameof(Index));
        }
    }
}
