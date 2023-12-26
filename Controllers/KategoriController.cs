using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WP_Project.Data;
using WP_Project.Models;

namespace WP_Project.Controllers
{
    public class KategoriController : Controller
    {
        private readonly AppDbContext _dbContext;

        public KategoriController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult AnaKategoriEkle()
        {
            return View();
        }
        /// <summary>
        /// Ana Kategori
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AnaKategoriEkle(AnaKategoriModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _dbContext.Ana_Kategori.Add(model);
                    _dbContext.SaveChanges();

                    ViewBag.SuccessMessage = "Ana kategori başarıyla eklendi.";
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = $"Hata oluştu: {ex.Message}";
                }
            }
            else
            {
                ViewBag.ErrorMessage = "Formda eksik veya hatalı bilgiler var.";
            }

            return View();
        }

        public IActionResult AnaKategoriListele()
        {
            var anaKategoriler = _dbContext.Ana_Kategori.ToList();
            return View(anaKategoriler);
        }

        public IActionResult SilAnaKategori(int id)
        {
            var anaKategori = _dbContext.Ana_Kategori.Find(id);
            if (anaKategori == null)
            {
                return NotFound();
            }

            _dbContext.Ana_Kategori.Remove(anaKategori);
            _dbContext.SaveChanges();

            return RedirectToAction("AnaKategoriListele");
        }

        [HttpGet]
        public IActionResult AnaKategoriGuncelle(int id)
        {
            var anaKategori = _dbContext.Ana_Kategori.Find(id);
            if (anaKategori == null)
            {
                return NotFound();
            }

            return View("AnaKategoriGuncelle", anaKategori);
        }

        [HttpPost]
        public IActionResult AnaKategoriGuncelle(AnaKategoriModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var existingAnaKategori = _dbContext.Ana_Kategori.Find(model.KatID);

                    if (existingAnaKategori != null)
                    {
                        existingAnaKategori.Kat_Ad = model.Kat_Ad;

                        _dbContext.Ana_Kategori.Update(existingAnaKategori);
                        _dbContext.SaveChanges();
                       
                        return RedirectToAction("AnaKategoriListele");
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = $"Güncelleme sırasında bir hata oluştu: {ex.Message}";
                }
            }

            return View("AnaKategoriGuncelle", model);
        }
       
       


    }
}

