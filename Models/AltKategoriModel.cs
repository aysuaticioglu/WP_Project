using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WP_Project.Models
{
    public class AltKategoriModel
    {
        public int AltKatID { get; set; }

        public int KatID { get; set; }
        [StringLength(255)]
        public string AltKat_Ad { get; set; }

        // Alt kategori ile ilişkiyi temsil eden özellik
        public AnaKategoriModel AnaKategori { get; set; }

      
    }

}
