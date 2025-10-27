using System.ComponentModel.DataAnnotations;

namespace GoallyticsApp.UI.Models.Auth
{
    public class UserLoginModel
    {
        [Required(ErrorMessage ="Email alanı boş bırakılamaz.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Şifre alanı boş bırakılamaz.")]
        public string Password { get; set; }
    }
}
