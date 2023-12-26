
using WP_Project.Models;
using Microsoft.AspNetCore.Mvc;
using WP_Project.Data;



public class LoginController : Controller
{
    private readonly AppDbContext _dbContext;

    public LoginController(AppDbContext dbContext)
     {
        _dbContext = dbContext;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }
    [HttpPost]
    public IActionResult Index(LoginModel model)
    {
        if (ModelState.IsValid)
        {
            // Şifre ve e-postayı veritabanında kontrol etmek için gerekli işlemleri yapın
            var user = _dbContext.Kullanici.FirstOrDefault(l => l.Kullanici_Ad == model.Kullanici_Ad && l.Parola == model.Parola);

            if (user != null)
            {
                TempData["Kullanici_Ad"] = user.Kullanici_Ad;
                // Giriş başarılı, yönlendirme yapabilirsiniz
                return RedirectToAction("Index", "Urunler");
            }
            else
            {
                // Giriş başarısız, hata mesajı gösterilebilir
                ViewBag.ErrorMessage = "Geçersiz giriş denemesi. Lütfen doğru e-posta adresi ve şifre girin.";

            }
        }

        return View(model);
    }
}
    

  