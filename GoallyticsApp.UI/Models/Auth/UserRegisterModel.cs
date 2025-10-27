using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace GoallyticsApp.UI.Models.Auth
{
    public class UserRegisterModel
    {
        [Required(ErrorMessage = " Ad alanı boş bırakılamaz.")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Soyad alanı boş bırakılamaz.")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Kullanıcı adı alanı boş bırakılamaz.")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Email alanı boş bırakılamaz.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Şifre alanı boş bırakılamaz.")]
        [StringLength(50,MinimumLength =6,ErrorMessage ="Şifre alanı en az 6 karakter olmalı.")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Şifre alanı boş bırakılamaz.")]
        [Compare("Password",ErrorMessage ="Şifreler eşleşmiyor.")]
        public string RePassword { get; set; }
        [Required(ErrorMessage = "Email alanı boş bırakılamaz.")]

        public DateTime? DateOfBirth { get; set; } = DateTime.Now;
        public int? AppRoleId { get; set; }
        public int GenderId { get; set; }
        public SelectList? Genders { get; set; }
    }
}
