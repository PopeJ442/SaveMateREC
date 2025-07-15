 
using System.ComponentModel.DataAnnotations;
 
namespace Savemate.Domain
{
    public class User 
    {
        
        public required string UserName { get; set; }
        public required string Email { get; set; }

        public required string Password { get; set; }
        [Compare("Password")]
        public required string ConfirmPassword { get; set; }
    }
}
