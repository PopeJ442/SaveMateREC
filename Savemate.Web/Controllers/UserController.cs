using Microsoft.AspNetCore.Mvc;
using Savemate.Application.Common.Extensions;
using Savemate.Application.Services.IService;
using Savemate.Domain.Entities;
using Savemate.Infrastructure;
using System.Threading.Tasks;

namespace Savemate.Web.Controllers
{
    public class UserController(IApplicationUserService userService) : Controller
    {
        private readonly IApplicationUserService _userService = userService;

        int age ;
        public async Task<IActionResult> Index()
        {
           var users = await _userService.GetAllUsers();

            return View(users);
        }
        public IActionResult Details(string id)
        {
            var user = _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound();
            return View(user);
        }

        public IActionResult Create() 
        {
        return View();
        }
      
        public async Task<IActionResult> Detail(string id)
        {



            var user = await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound();

            age = user.DOB.CalculateAge();

            // age = DateTime.Now.Year - user.DOB.Year;


            return View(user);

        }

        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound();

            return View(user);
        }


        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound();
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ApplicationUser user) 
        {
            if (!ModelState.IsValid) 
            {
            return View(user);
            }
            await _userService.AddUserAsync(user);
           return  RedirectToAction("Index");
        
        }
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ApplicationUser user)
        {
            if (!ModelState.IsValid) return NotFound();

          await  _userService.UpdateUserAsync(user);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(ApplicationUser user)
        {
             await _userService.DeleteUserAsync(user);
            return RedirectToAction(nameof(Index));

        }
        
       
             
    }
}
