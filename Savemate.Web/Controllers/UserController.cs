using Microsoft.AspNetCore.Mvc;
using Savemate.Application.Services.IService;
using Savemate.Domain.Entities;
using System.Threading.Tasks;

namespace Savemate.Web.Controllers
{
    public class UserController(IUserService userService) : Controller
    {
        private readonly IUserService _userService = userService;
         
        public async Task<IActionResult> Index()
        {
           var users = await _userService.GetAllUsers();

            return View(users);
        }
        public IActionResult Details(Guid id)
        {
            var user = _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound();
            return View(user);
        }

        public IActionResult Create() 
        {
        return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(User user) 
        {
            if (!ModelState.IsValid) 
            {
            return View(user);
            }
            await _userService.AddUserAsync(user);
           return  RedirectToAction("Index");
        
        }
        public async Task<IActionResult> Edit(Guid id)
        {
            var user = _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound();
          
            return View(user);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(User user)
        {
            if (!ModelState.IsValid) return NotFound();
            _userService.UpdateUserAsync(user);
            return RedirectToAction("Index");
        }
        
        public IActionResult Delete(Guid id)
        {
            var user =   _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound();
            return View(user);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmation(User user)
        {
              _userService.DeleteUserAsync(user);
            return RedirectToAction(nameof(Index));

        }
    }
}
