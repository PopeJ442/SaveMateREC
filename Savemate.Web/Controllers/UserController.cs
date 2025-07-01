using Microsoft.AspNetCore.Mvc;
using Savemate.Application.Services.IService;
using Savemate.Domain.Entities;
using System.Threading.Tasks;

namespace Savemate.Web.Controllers
{
    public class UserController(IUserService userService) : Controller
    {
        private readonly IUserService _userService = userService;

        int age=9;
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
            var user =await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound();
          
            return View(user);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(User user)
        {
            if (!ModelState.IsValid) return NotFound();

          await  _userService.UpdateUserAsync(user);
            return RedirectToAction("Index");
        }
        
        public async Task<IActionResult> Delete(Guid id)
        {
            var user = await  _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound();
            return View(user);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmation(User user)
        {
             await _userService.DeleteUserAsync(user);
            return RedirectToAction(nameof(Index));

        }
        
        [HttpGet]
        public async Task<IActionResult> Detail(Guid id) 
        {
         
           
           
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound();
            age = DateTime.Now.Year - user.DOB.Year;

            if (user.DOB.ToDateTime(TimeOnly.MinValue) > DateTime.Today.AddYears(-age))
            {
                age--;
            }
            ViewData["age"] = age;
            return View(user);

        }
             
    }
}
