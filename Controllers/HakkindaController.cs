using Microsoft.AspNetCore.Mvc;
using WP_Project.Data;
using WP_Project.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace WP_Project.Controllers
{
    public class HakkindaController : Controller
    {
        private readonly AppDbContext _dbContext;

        public HakkindaController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult Index()
        {
            // Mevcut "Hakkımızda" içeriğini veritabanından al
            var hakkimizdaModel = _dbContext.Hakkinda.FirstOrDefault();

            // Eğer varsa, modeli view'a gönder
            if (hakkimizdaModel != null)
            {
                return View(hakkimizdaModel);
            }

            // Eğer "Hakkımızda" içeriği yoksa, boş bir modeli view'a gönder
            return View(new HakkindaModel());
        }

        [HttpPost]
        public IActionResult Index(HakkindaModel model)
        {
            if (ModelState.IsValid)
            {
                // Eğer "Hakkımızda" içeriği daha önce eklenmemişse, yeni bir tane oluştur
                var existingHakkimizdaModel = _dbContext.Hakkinda.FirstOrDefault();
                if (existingHakkimizdaModel == null)
                {
                    _dbContext.Hakkinda.Add(model);
                }
                else
                {
                    // Eğer daha önce eklenmişse, mevcut içeriği güncelle
                    existingHakkimizdaModel.Hakkinda_Baslik = model.Hakkinda_Baslik;
                    existingHakkimizdaModel.Hakkinda_Icerik = model.Hakkinda_Icerik;
                    _dbContext.Hakkinda.Update(existingHakkimizdaModel);
                }

                _dbContext.SaveChanges();

                ViewBag.SuccessMessage = "Hakkımızda başarıyla eklendi/düzenlendi.";
            }
            else
            {
                ViewBag.ErrorMessage = "Hata Oluştu.";
            }

            return View(model);
        }

    }
}
