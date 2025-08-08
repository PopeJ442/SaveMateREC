using System.ComponentModel.DataAnnotations;

namespace Savemate.Web.Models
{
    public class TwoFactor
    {
        [Required]
        public string TwoFactorCode { get; set; }
    }
}
