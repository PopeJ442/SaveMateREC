using Microsoft.AspNetCore.Mvc;
 

namespace Savemate.Web.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            
            return View();
        }
    }
}
