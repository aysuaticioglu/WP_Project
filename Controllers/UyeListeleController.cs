using Microsoft.AspNetCore.Mvc;
using System.Linq;
using WP_Project.Data;

public class UyeListeleController : Controller
{
    private readonly AppDbContext _dbContext;

    public UyeListeleController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IActionResult Index()
    {
        var uyeler = _dbContext.Uye.ToList();
        return View(uyeler);
    }
}