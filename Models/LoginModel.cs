using System.ComponentModel.DataAnnotations;

namespace WP_Project.Models
{
    public class LoginModel
    {
        public int KulGirisID { get; set; }
        [Required(ErrorMessage = "Kullanıcı Adı zorunlu bir alan.")]
        public string? Kullanici_Ad { get; set; }

        [Required(ErrorMessage = "Parola zorunlu bir alan.")]
        public string? Parola { get; set; }
    }
}