using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WP_Project.Data;
using WP_Project.Models;

namespace WP_Project.Controllers
{
    public class AltKategoriController : Controller
    {
        private readonly AppDbContext _dbContext;

        public AltKategoriController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult AltKategoriEkle()
        {
            ViewBag.AnaKategoriler = new SelectList(_dbContext.Ana_Kategori.ToList(), "KatID", "Kat_Ad");
            return View();
        }

        [HttpPost]
        public IActionResult AltKategoriEkle(AltKategoriModel model)
        {

            try
            {
                _dbContext.Alt_Kategori.Add(model);
                _dbContext.SaveChanges();

                ViewBag.SuccessMessage = "Alt kategori başarıyla eklendi.";
                ViewBag.AnaKategoriler = new SelectList(_dbContext.Ana_Kategori.ToList(), "KatID", "Kat_Ad");

                return RedirectToAction("AltKategoriListele", new { anaKategoriId = model.KatID });
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Alt kategori eklenirken bir hata oluştu: {ex.Message}";
            }


            ViewBag.AnaKategoriler = new SelectList(_dbContext.Ana_Kategori.ToList(), "KatID", "Kat_Ad");
            return View();
        }

        public IActionResult AltKategoriListele(int KatID)
        {
            try
            {
                var altKategoriler = _dbContext.Alt_Kategori
    .Include(a => a.AnaKategori)  // Include the main category

    .ToList();



                return View(altKategoriler);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Veriler getirilirken bir hata oluştu: {ex.ToString()}";
                return View("Error"); // Burada View döndürülmediği için sayfa görüntülenmiyor.
            }



        }

        public IActionResult SilAltKategori(int id)
        {
            var altKategori = _dbContext.Alt_Kategori.Find(id);
            if (altKategori == null)
            {
                return NotFound();
            }

            _dbContext.Alt_Kategori.Remove(altKategori);
            _dbContext.SaveChanges();

            return RedirectToAction("AltKategoriListele", new { anaKategoriId = altKategori.KatID });
        }

        [HttpGet]
        public IActionResult AltKategoriGuncelle(int id)
        {
            var altKategori = _dbContext.Alt_Kategori.Find(id);
            if (altKategori == null)
            {
                return NotFound();
            }

            ViewBag.AnaKategoriler = new SelectList(_dbContext.Ana_Kategori.ToList(), "KatID", "Kat_Ad");
            return View(altKategori);
        }

        [HttpPost]
        public IActionResult AltKategoriGuncelle(AltKategoriModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Find the existing AltKategori by ID
                    var existingAltKategori = _dbContext.Alt_Kategori.Find(model.AltKatID);

                    if (existingAltKategori != null)
                    {
                        // Update other properties with the values from the form
                        existingAltKategori.AltKat_Ad = model.AltKat_Ad;

                        // If you want to update the AnaKategori as well
                        existingAltKategori.KatID = model.KatID;

                        // Save changes to the database
                        _dbContext.Update(existingAltKategori);
                        _dbContext.SaveChanges();

                        return RedirectToAction("AltKategoriListele", new { anaKategoriId = model.KatID });
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"Veriler getirilirken bir hata oluştu: {ex.Message}";
                    return View();
                }
            }

            ViewBag.AnaKategoriler = new SelectList(_dbContext.Ana_Kategori.ToList(), "KatID", "Kat_Ad");
            return View(model);
        }
    }
}
