using Microsoft.EntityFrameworkCore;
using WP_Project.Models;

namespace WP_Project.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<LoginModel> Kullanici { get; set; }
        public DbSet<UyeModel> Uye { get; set; }
        public DbSet<UrunModel> Urun { get; set; }
        public DbSet<AnaKategoriModel> Ana_Kategori { get; set; }
        
        public DbSet<AltKategoriModel> Alt_Kategori { get; set; }
        public DbSet<HakkindaModel> Hakkinda { get; set; }
        public DbSet<IletisimModel> Iletisim{ get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LoginModel>().HasKey(l => l.KulGirisID);
            modelBuilder.Entity<UyeModel>().HasKey(u => u.UyeID);
            modelBuilder.Entity<UrunModel>().HasKey(u => u.UrunID);
            modelBuilder.Entity<AnaKategoriModel>().HasKey(k => k.KatID);
            modelBuilder.Entity<AltKategoriModel>().HasKey(a => a.AltKatID);

            // AnaKategoriModel ve AltKategoriModel arasındaki ilişki
            modelBuilder.Entity<AltKategoriModel>()
                .HasOne(alt => alt.AnaKategori)
                .WithMany(ana => ana.AltKategoriler)
                .HasForeignKey(alt => alt.KatID);

            modelBuilder.Entity<AnaKategoriModel>().HasData(
                new AnaKategoriModel { KatID = 1, Kat_Ad = "Örnek Kategori 1" },
                new AnaKategoriModel { KatID = 2, Kat_Ad = "Örnek Kategori 2" }
            );

            modelBuilder.Entity<AltKategoriModel>().HasData(
                new AltKategoriModel { AltKatID = 1, KatID = 1, AltKat_Ad = "Örnek Alt Kategori 1" },
                new AltKategoriModel { AltKatID = 2, KatID = 1, AltKat_Ad = "Örnek Alt Kategori 2" }
            );


            base.OnModelCreating(modelBuilder);
        }
    }
}
