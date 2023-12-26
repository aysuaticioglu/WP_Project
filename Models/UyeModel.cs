using System;
using System.ComponentModel.DataAnnotations;

namespace WP_Project.Models
{
    public class UyeModel
    {
        public int UyeID { get; set; }
        public string? Kul_Ad { get; set; }
        public string? Ad { get; set; }
        public string? Soyad { get; set; }

        [EmailAddress(ErrorMessage = "Geçersiz email adresi.")]
        [DisplayFormat(NullDisplayText = "example@example.com")]
        public string? Email { get; set; }

        [DataType(DataType.Password)]
        public string? Sifre { get; set; }

        public int Cinsiyet { get; set; }

        [Display(Name = "Üye Tarihi")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime? Uye_Tarihi { get; set; }


    }
}
