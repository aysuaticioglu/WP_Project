
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WP_Project.Models
{
    public class IletisimModel
    {
        [Key]
        public int IletisimID { get; set; }

        [Required(ErrorMessage = "Ad alanı zorunlu.")]
        [StringLength(45, ErrorMessage = "Ad alanı en fazla {1} karakter uzunluğunda olmalıdır.")]
        public string Ad { get; set; }

        [Required(ErrorMessage = "Soyad alanı zorunlu.")]
        [StringLength(45, ErrorMessage = "Soyad alanı en fazla {1} karakter uzunluğunda olmalıdır.")]
        public string Soyad { get; set; }

        [Required(ErrorMessage = "Konu alanı zorunlu.")]
        [StringLength(45, ErrorMessage = "Konu alanı en fazla {1} karakter uzunluğunda olmalıdır.")]
        public string Konu { get; set; }

        [Required(ErrorMessage = "Mesaj alanı zorunlu.")]
        [StringLength(1000, ErrorMessage = "Mesaj alanı en fazla {1} karakter uzunluğunda olmalıdır.")]
        public string Mesaj { get; set; }
    }
}
