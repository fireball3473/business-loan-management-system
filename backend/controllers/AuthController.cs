using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;




[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly FinansDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthController(FinansDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    
    
    
    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginDto request)
    {
        var kullanici = _context.T_Kullanicilar.FirstOrDefault(u => u.KullaniciAdi == request.KullaniciAdi);
        
        if (kullanici == null || !BCrypt.Net.BCrypt.Verify(request.Sifre, kullanici.Sifre))
        {
            return Unauthorized(new { message = "Kullanıcı adı veya şifre hatalı!" });
        }

        var jwtSettings = _configuration.GetSection("Jwt");
        var secretKey = jwtSettings["Key"];
        if (string.IsNullOrEmpty(secretKey))
        {
            secretKey = "xryapoabhbliidprpgbljnsrwdynetnbmoovcpbsrikmdukh"; 
        }

        var key = Encoding.ASCII.GetBytes(secretKey);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, kullanici.KullaniciId.ToString()),
            new Claim(ClaimTypes.Name, kullanici.KullaniciAdi),
            new Claim("AdSoyad", kullanici.AdSoyad ?? "")
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(2),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return Ok(new 
        { 
            Token = tokenHandler.WriteToken(token),
            KullaniciAdSoyad = kullanici.AdSoyad
        });
    }
}
