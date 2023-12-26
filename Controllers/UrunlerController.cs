// UrunlerController.cs
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WP_Project.Data;
using WP_Project.Models;


public class UrunlerController : Controller
{
    private readonly AppDbContext _dbContext;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public UrunlerController(AppDbContext dbContext,IWebHostEnvironment webHostEnvironment)
    {
        _dbContext = dbContext;
        _webHostEnvironment = webHostEnvironment;
    }

    [HttpGet]
    public IActionResult Listele()
    {
        try
        {
            // Include the related data (main category and subcategory)
            var urunler = _dbContext.Urun
                .Include(u => u.AnaKategoriModel)  // Include the main category
                .Include(u => u.AltKategoriModel)  // Include the subcategory
                .ToList();

            return View(urunler);
        }
        catch (Exception ex)
        {
            ViewBag.ErrorMessage = "Veriler getirilirken bir hata oluştu: " + ex.Message;
            return View();
        }
    }

    [HttpGet]
    public IActionResult Index()
    {
        try
        {
            var anaKategoriler = _dbContext.Ana_Kategori.ToList();

            ViewBag.AnaKategoriler = new SelectList(anaKategoriler, "KatID", "Kat_Ad");
            ViewBag.AltKategoriler = new SelectList(new List<AltKategoriModel>(), "AltKatID", "AltKat_Ad");

            return View();
        }
        catch (Exception ex)
        {
            ViewBag.ErrorMessage = "Sayfa yüklenirken bir hata oluştu: " + ex.Message;
            return View();
        }
    }

    [HttpPost]
    public IActionResult Index(UrunModel model, IFormFile Resim)
    {
        try
        {
            if (model.KatID == 0)
            {
                ViewBag.ErrorMessage = "Lütfen bir ana kategori seçiniz.";
                ViewBag.AnaKategoriler = new SelectList(_dbContext.Ana_Kategori.ToList(), "KatID", "Kat_Ad");
                return View();
            }

            var altKategoriler = _dbContext.Alt_Kategori.Where(a => a.KatID == model.KatID).ToList();
            ViewBag.AltKategoriler = new SelectList(altKategoriler, "AltKatID", "AltKat_Ad");

            // Resim yükleme işlemleri
            if (Resim != null)
            {
                Console.WriteLine($"Resim: {Resim}");
                Console.WriteLine($"Renk_Kod: {model.Renk_Kod}");
                var resimAdi = Guid.NewGuid().ToString() + Path.GetExtension(Resim.FileName);
                var resimYolu = Path.Combine(_webHostEnvironment.WebRootPath, "images", resimAdi);

                using (var stream = new FileStream(resimYolu, FileMode.Create))
                {
                    Resim.CopyTo(stream);
                }

                model.Resim = "/images/" + resimAdi;
            }

            _dbContext.Urun.Add(model);
            _dbContext.SaveChanges();

            ViewBag.SuccessMessage = "Ürün başarıyla eklendi.";

            return RedirectToAction("Listele");
        }
        catch (Exception ex)
        {
            ViewBag.ErrorMessage = "Ürün eklenirken bir hata oluştu: " + ex.Message;
            return View();
        }
    }


    [HttpGet]
    public JsonResult GetAltKategoriler(int KatID)
    {
        try
        {
            // Seçilen ana kategoriye ait alt kategorileri JSON olarak döndür
            var altKategoriler = _dbContext.Alt_Kategori
                .Where(a => a.KatID == KatID)
                .Select(alt => new { AltKatID = alt.AltKatID, AltKat_Ad = alt.AltKat_Ad })
                .ToList();

            return Json(altKategoriler);
        }
        catch (Exception ex)
        {
            return Json(new { error = "Alt kategoriler alınırken bir hata oluştu: " + ex.Message });
        }
    }





    [HttpGet]
    public IActionResult Guncelle(int id)
    {
        try
        {
            var existingUrun = _dbContext.Urun.Find(id);

           
            if (existingUrun != null)
            {
                // Fetch the available main categories
                var anaKategoriler = _dbContext.Ana_Kategori.ToList();
                ViewBag.AnaKategoriler = new SelectList(anaKategoriler, "KatID", "Kat_Ad");

                // Set AltKategoriler to an empty list initially
                ViewBag.AltKategoriler = new SelectList(new List<AltKategoriModel>(), "AltKatID", "AltKat_Ad");


                var altKategoriler = _dbContext.Alt_Kategori.Where(a => a.KatID == existingUrun.KatID).ToList();
                ViewBag.AltKategoriler = new SelectList(altKategoriler, "AltKatID", "AltKat_Ad");
               

                return View(existingUrun);

            }
            else
            {
                ViewBag.ErrorMessage = "Ürün bulunamadı.";
                return RedirectToAction("Listele"); // Redirect to the list view if the product is not found
            }
        }
        catch (Exception ex)
        {
            ViewBag.ErrorMessage = "Ürün güncellenirken bir hata oluştu: " + ex.Message;
            return RedirectToAction("Listele"); // Redirect to the list view in case of an error
        }
    }

   
    [HttpPost]
    public IActionResult Guncelle(UrunModel model, IFormFile Resim)
    {
        try
        {
            // Find the existing product by ID
            var existingUrun = _dbContext.Urun.Find(model.UrunID);

            if (existingUrun != null)
            {
                // Update other properties with the values from the form
                existingUrun.Urun_Ad = model.Urun_Ad;
                existingUrun.Fiyat = model.Fiyat;
                
                existingUrun.Stok_Adet = model.Stok_Adet;

                // If you have relationships, update them as well
                existingUrun.KatID = model.KatID;
                existingUrun.AltKatID = model.AltKatID;

                // Example: Update related models
                // Assuming that KatID is the foreign key for AnaKategoriModel
                existingUrun.AnaKategoriModel = _dbContext.Ana_Kategori.Find(model.KatID);

                // Assuming that AltKatID is the foreign key for AltKategoriModel
                existingUrun.AltKategoriModel = _dbContext.Alt_Kategori.Find(model.AltKatID);

                // Resim yükleme işlemleri sadece yeni bir Resim geldiğinde yapılır
                if (Resim != null)
                {
                    var resimAdi = Guid.NewGuid().ToString() + Path.GetExtension(Resim.FileName);
                    var resimYolu = Path.Combine(_webHostEnvironment.WebRootPath, "images", resimAdi);

                    using (var stream = new FileStream(resimYolu, FileMode.Create))
                    {
                        Resim.CopyTo(stream);
                    }

                    existingUrun.Resim = "/images/" + resimAdi;
                }
                existingUrun.Detay = model.Detay;
                // ModelState'i temizle
                if (!model.Renk_Kod.StartsWith("#"))
                {
                    model.Renk_Kod = "#" + model.Renk_Kod;
                }
                existingUrun.Renk_Kod = model.Renk_Kod;
                ModelState.Clear();

                // Check for validation errors before saving changes
                if (!TryValidateModel(existingUrun))
                {
                    // If validation fails, handle errors
                    // ...
                    return View(model);
                }

                // Save changes to the database
                _dbContext.Update(existingUrun);
                _dbContext.SaveChanges();

                // Redirect to the list view after successful update
                return RedirectToAction("Listele", "Urunler");
            }
            else
            {
                ViewBag.ErrorMessage = "Ürün bulunamadı.";
                return View();
            }
        }
        catch (Exception ex)
        {
            ViewBag.ErrorMessage = "Veriler getirilirken bir hata oluştu: " + ex.Message;
            return View();
        }
    }


    public IActionResult Sil(int id)
    {
        // ID'ye göre ürünü sil ve listeleme sayfasına yönlendir
        var urun = _dbContext.Urun.Find(id);

        if (urun != null)
        {
            _dbContext.Urun.Remove(urun);
            _dbContext.SaveChanges();
        }

        return RedirectToAction("Listele");
    }

}
