using System.ComponentModel.DataAnnotations;

namespace Mocoding.EasyDocDb.CrudSample.ViewModels.Account
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
