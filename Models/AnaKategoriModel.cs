using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WP_Project.Models
{
    public class AnaKategoriModel
    {
        public int KatID { get; set; }

        [Required(ErrorMessage = "Ana Kategori Adı zorunlu bir alandır.")]
        public string Kat_Ad { get; set; }

        // Ana kategori ile ilişkiyi temsil eden özellik
        public ICollection<AltKategoriModel> AltKategoriler { get; set; } = new List<AltKategoriModel>();
    }
}

