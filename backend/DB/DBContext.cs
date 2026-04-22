using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

/// <summary>
/// Veritabanı bağlantısı ve Entity Framework Core tablolarını temsil eden ana bağlam (Context) sınıfı.
/// </summary>
public class FinansDbContext : DbContext // DBSet veritabanındaki bir tabloyu temsil eder.
{
    public FinansDbContext(DbContextOptions<FinansDbContext> options) : base(options) { }

    public DbSet<Musteri> T_Musteriler { get; set; }
    public DbSet<MevduatHesap> T_MevduatHesaplari { get; set; }
    public DbSet<KrediHesap> T_Krediler { get; set; }
    public DbSet<Tahsilat> T_Tahsilatlar { get; set; }
    public DbSet<Kullanici> T_Kullanicilar { get; set; }

    /// <summary>
    /// Veritabanı tabloları oluşturulurken uygulanacak konfigürasyonlar (örn: VKN alanının unique olması).
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Musteri>()
            .HasIndex(m => m.VKN)
            .IsUnique(); // Aynı anda birden fazla kayıt oluşturulmasını engeller.

        base.OnModelCreating(modelBuilder);
    }
}

public class FinansDbContextFactory : IDesignTimeDbContextFactory<FinansDbContext> // Migration oluştururken DBContext örneğini oluşturur ve Migration'ları yönetir.
{
    public FinansDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<FinansDbContext>();
        optionsBuilder.UseNpgsql("Host=localhost;Database=loanapp;Username=postgres;Password=201079");

        return new FinansDbContext(optionsBuilder.Options);
    }
}