using BCrypt.Net;

// Seeder: Rastgele veriler yerleştirmesini sağlayan araç
public static class DbSeeder
{
    public static void Seed(FinansDbContext context)
    {
        if (!context.T_Kullanicilar.Any())
        {
            var adminUser = new Kullanici
            {
                KullaniciAdi = "admin",
                Sifre = BCrypt.Net.BCrypt.HashPassword("123456"),
                AdSoyad = "admin"
            };
            context.T_Kullanicilar.Add(adminUser);
            context.SaveChanges();
        }

        if (!context.T_Musteriler.Any())
        {
            var ornekMusteri = new Musteri
            {
                VKN = "1111111111",
                Unvan = "Test Yazılım Danışmanlık A.Ş.",
                Sektor = "Bilişim",
                MevduatHesaplari = null!,
                Krediler = null!
            };
            
            context.T_Musteriler.Add(ornekMusteri);
            context.SaveChanges();

            var ornekHesap1 = new MevduatHesap
            {
                MusteriId = ornekMusteri.MusteriId,
                Musteri = null!,
                IbanNo = "TR000000000000012345678901",
                Bakiye = 500000m,
                DovizCinsi = "TRY"
            };

            var ornekHesap2 = new MevduatHesap
            {
                MusteriId = ornekMusteri.MusteriId,
                Musteri = null!,
                IbanNo = "TR000000000000012345678902",
                Bakiye = 25000m,
                DovizCinsi = "USD"
            };

            context.T_MevduatHesaplari.AddRange(ornekHesap1, ornekHesap2);
            context.SaveChanges();
        }
    }
}
