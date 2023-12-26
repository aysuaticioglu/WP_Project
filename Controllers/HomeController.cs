// HomeController.cs
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WP_Project.Data;
using WP_Project.Models;


namespace WP_Project.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext _dbContext;

    public HomeController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IActionResult Index()
    {
        // Fetch the initial data (e.g., categories)
        var categories = _dbContext.Ana_Kategori.ToList();
        // Adjust based on your actual repository
        ViewBag.Categories = categories;
        return View(categories);
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }


    // Kategoriye bağlı ürünleri getirme
    public IActionResult Icerik(int? KatID)
    {
        IQueryable<UrunModel> products;

        if (KatID.HasValue)
        {
            // Kategoriye bağlı ürünleri getir
            products = _dbContext.Urun
                .Where(p => p.KatID == KatID.Value);
        }
        else
        {
            // Kategoriye bağlı olmayan bütün ürünleri getir
            products = _dbContext.Urun;
        }

        return View(products.ToList());
    }

    // Tüm ürünleri getirme
    public IActionResult TumUrunler()
    {
        var tumUrunler = _dbContext.Urun.ToList();
        return View("Icerik", tumUrunler);
    }
    public IActionResult Hakkinda()
    {
        // Veritabanından HakkindaModel verilerini çekiyoruz
        var hakkindaModel = _dbContext.Hakkinda.FirstOrDefault();

        // Eğer veri yoksa varsayılan bir değer atayabilirsiniz
        if (hakkindaModel == null)
        {
            hakkindaModel = new HakkindaModel
            {
                Hakkinda_Baslik = "Başlık Buraya",
                Hakkinda_Icerik = "İçerik Buraya"
            };
        }

        return View(hakkindaModel);
    }
    public IActionResult Iletisim()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Iletisim(IletisimModel model)
    {
        if (ModelState.IsValid)
        {
            _dbContext.Iletisim.Add(model);
            _dbContext.SaveChanges();

            ViewBag.SuccessMessage = "İletişim formu başarıyla gönderildi!";
            ModelState.Clear();

            return View();
        }

        return View(model);
    }

}
