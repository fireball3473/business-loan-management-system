using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;




public class FinansDbContext : DbContext 
{
    public FinansDbContext(DbContextOptions<FinansDbContext> options) : base(options) { }

    public DbSet<Musteri> T_Musteriler { get; set; }
    public DbSet<MevduatHesap> T_MevduatHesaplari { get; set; }
    public DbSet<KrediHesap> T_Krediler { get; set; }
    public DbSet<Tahsilat> T_Tahsilatlar { get; set; }
    public DbSet<Kullanici> T_Kullanicilar { get; set; }

    
    
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Musteri>()
            .HasIndex(m => m.VKN)
            .IsUnique(); 

        base.OnModelCreating(modelBuilder);
    }
}

public class FinansDbContextFactory : IDesignTimeDbContextFactory<FinansDbContext> 
{
    public FinansDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<FinansDbContext>();
        optionsBuilder.UseNpgsql("Host=localhost;Database=loanapp;Username=postgres;Password=201079");

        return new FinansDbContext(optionsBuilder.Options);
    }
}