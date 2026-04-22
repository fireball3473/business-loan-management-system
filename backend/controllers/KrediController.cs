using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

[Authorize]
[ApiController]
[Route("api/[controller]")]



public class KrediController : ControllerBase
{
    private readonly FinansDbContext _context;

    public KrediController(FinansDbContext context)
    {
        _context = context;
    }

    
    
    
    [HttpGet("musteri-ara/{vkn}")]
    public async Task<IActionResult> GetMusteriByVkn(string vkn)
    {
        var musteri = await _context.T_Musteriler
            .Where(m => m.VKN == vkn)
            .Select(m => new { m.MusteriId, m.Unvan })
            .FirstOrDefaultAsync();

        if (musteri == null) return NotFound("Müşteri bulunamadı.");
        return Ok(musteri);
    }

    
    
    
    
    [HttpPost("kredi-olustur")]
    public async Task<IActionResult> KrediOlustur([FromBody] KrediOlusturDto dto)
    {
        var musteri = await _context.T_Musteriler.FirstOrDefaultAsync(m => m.VKN == dto.VKN);
        if (musteri == null) return BadRequest("Geçersiz VKN.");

        decimal toplamBorc = dto.AnaPara + (dto.AnaPara * (dto.FaizOrani / 100) * dto.VadeSayisi);

        var yeniKredi = new KrediHesap
        {
            MusteriId = musteri.MusteriId,
            Musteri = null!,
            KrediTuru = dto.KrediTuru,
            AnaPara = dto.AnaPara,
            DovizCinsi = dto.DovizCinsi,
            OdemeTipi = dto.OdemeTipi,
            VadeSayisi = dto.VadeSayisi,
            FaizOrani = dto.FaizOrani,
            ToplamBorc = toplamBorc,
            KalanBorc = toplamBorc,
            BaslangicTarihi = DateTime.UtcNow
        };

        _context.T_Krediler.Add(yeniKredi);
        await _context.SaveChangesAsync();

        return Ok(new { Message = "Kredi başarıyla onaylandı ve kaydedildi.", KrediId = yeniKredi.KrediId, HesaplananBorc = toplamBorc });
    }
}