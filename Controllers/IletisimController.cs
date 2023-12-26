using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using WP_Project.Data; // Veritabanı bağlantısını sağlayan context sınıfı
using WP_Project.Models;

namespace WP_Project.Controllers
{
    public class IletisimController : Controller
    {
        private readonly AppDbContext _dbContext;

        public IletisimController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            try
            {
                var iletisimKayitlari = _dbContext.Iletisim.ToList();
                return View(iletisimKayitlari);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Veriler getirilirken bir hata oluştu: {ex.Message}";
                return View("Error"); // Burada View döndürülmediği için sayfa görüntülenmiyor.
            }
        }
    }
}
