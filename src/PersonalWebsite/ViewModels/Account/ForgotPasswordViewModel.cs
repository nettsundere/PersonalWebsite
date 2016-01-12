using System.ComponentModel.DataAnnotations;

namespace PersonalWebsite.ViewModels.Account
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
