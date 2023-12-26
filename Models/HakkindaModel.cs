
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WP_Project.Models
{
    public class HakkindaModel
    { 
        [Key]
        public int HakkindaID { get; set; }

        [Required(ErrorMessage = "Başlık alanı zorunlu.")]
        public string Hakkinda_Baslik { get; set; }

        [Required(ErrorMessage = "İçerik alanı zorunlu.")]
        public string Hakkinda_Icerik { get; set; }
    }
}
