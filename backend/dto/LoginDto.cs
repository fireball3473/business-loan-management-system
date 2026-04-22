/// <summary>
/// Giriş sayfasından gönderilen (kullanıcı adı ve şifre) verilerini taşıyan nesne.
/// </summary>
public class LoginDto
{
    public required string KullaniciAdi { get; set; }
    public required string Sifre { get; set; }
}
