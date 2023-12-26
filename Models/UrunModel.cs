// UrunModel.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WP_Project.Models
{
    public class UrunModel
    {
        [Key]
        public int UrunID { get; set; }

        public int KatID { get; set; }
        public int AltKatID { get; set; }
       

        [ForeignKey("KatID")]
        public AnaKategoriModel AnaKategoriModel { get; set; }
 
        [ForeignKey("AltKatID")]
        public AltKategoriModel AltKategoriModel { get; set; }

        [Required(ErrorMessage = "Ürün Adı alanı boş bırakılamaz.")]
        [StringLength(255)]
        public string Urun_Ad { get; set; }

        public decimal Fiyat { get; set; }

        public string Renk_Kod { get; set; } 

        public int Stok_Adet { get; set; } = 0;

        [StringLength(500)]
        public string Resim { get; set; }

        [StringLength(1000)]
        public string Detay { get; set; }
    }
}
