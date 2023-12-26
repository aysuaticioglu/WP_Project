using Microsoft.AspNetCore.Mvc;
using WP_Project.Data;
using WP_Project.Models;

public class UyeController : Controller
{
    private readonly AppDbContext _dbContext;

    public UyeController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public IActionResult Index()
    {
        // Sayfanın ilk yüklenmesinde yapılacak işlemler (eğer gerekirse)
        return View();
    }

    [HttpGet]
    public IActionResult UyeOl()
    {
        // Üye olma sayfasını göster
        return View();
    }

    [HttpGet]
    public IActionResult UyeGiris()
    {
        // Üye giriş sayfasını göster
        return View();
    }

    [HttpPost]
    public IActionResult UyeOl(UyeModel model)
    {
        if (ModelState.IsValid)
        {
            // Geçerli model, kayıt işlemleri yapılabilir

            // Üye oluşturulduğu tarih ataması
            model.Uye_Tarihi = DateTime.Now;

            _dbContext.Uye.Add(model);
            _dbContext.SaveChanges();

            // Kayıt başarılı mesajı veya başka bir işlem
            ViewBag.SuccessMessage = "Üyelik başarıyla oluşturuldu!";

            

            // Başarılı kayıt sonrasında başka bir sayfaya yönlendirme yapabilirsiniz
            return RedirectToAction("UyeGiris", "Uye");
        }

        // ModelState geçerli değilse veya kayıt başarısızsa, aynı sayfaya geri dön
        return View("UyeOl", model);
    }

    [HttpPost]
    public IActionResult UyeGiris(UyeModel model)
    {
        if (ModelState.IsValid)
        {
            // Giriş işlemleri
            var user = _dbContext.Uye.FirstOrDefault(u => u.Email == model.Email && u.Sifre == model.Sifre);

            if (user != null)
            {
                // Giriş başarılı, kullanıcı adını TempData'ye taşı
             
                TempData["UyeGirisAd"] = user.Kul_Ad;
                
                ViewBag.SuccessMessage = "Geçersiz giriş denemesi. Lütfen doğru e-posta adresi ve şifre girin.";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                // Giriş başarısız, hata mesajı gösterilebilir
                ViewBag.ErrorMessage = "Geçersiz giriş denemesi. Lütfen doğru e-posta adresi ve şifre girin.";
            }
        }

        // ModelState geçerli değilse veya giriş başarısızsa, aynı sayfaya geri dön
        return View("UyeGiris");
    }
   
}
