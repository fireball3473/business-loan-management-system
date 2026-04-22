using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

[Authorize]
[ApiController]
[Route("api/[controller]")]



public class TahsilatController : ControllerBase
{
    private readonly FinansDbContext _context;

    public TahsilatController(FinansDbContext context)
    {
        _context = context;
    }

    
    
    
    [HttpGet("aktif-krediler/{vkn}")]
    public async Task<IActionResult> GetActiveLoans(string vkn)
    {
        var krediler = await _context.T_Krediler
            .Include(k => k.Musteri)
            .Where(k => k.Musteri.VKN == vkn && k.KalanBorc > 0)
            .Select(k => new {
                k.KrediId,
                k.KrediTuru,
                k.DovizCinsi,
                k.OdemeTipi,
                k.AnaPara,
                k.ToplamBorc,
                k.KalanBorc
            })
            .ToListAsync();

        return Ok(krediler);
    }

    
    
    
    [HttpGet("musteri-hesaplari/{krediId}")]
    public async Task<IActionResult> GetCustomerAccounts(int krediId)
    {
        var kredi = await _context.T_Krediler.FindAsync(krediId);
        if (kredi == null) return NotFound();

        var hesaplar = await _context.T_MevduatHesaplari
            .Where(h => h.MusteriId == kredi.MusteriId)
            .Select(h => new { h.HesapId, h.IbanNo, h.Bakiye, h.DovizCinsi })
            .ToListAsync();

        return Ok(hesaplar);
    }

    
    
    
    
    [HttpPost("tahsilat-yap")]
    public async Task<IActionResult> MakePayment([FromBody] TahsilatIslemDto dto)
    {
        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                var kredi = await _context.T_Krediler.FindAsync(dto.KrediId);
                var hesap = await _context.T_MevduatHesaplari.FindAsync(dto.HesapId);

                if (kredi == null || hesap == null) 
                    return BadRequest("Kredi veya hesap bulunamadı.");

                if (hesap.Bakiye < dto.TahsilatTutari)
                    return BadRequest("Hesap bakiyesi yetersiz!");

                if (dto.TahsilatTutari > kredi.KalanBorc)
                    return BadRequest("Tahsilat tutarı kalan borçtan büyük olamaz.");

                hesap.Bakiye -= dto.TahsilatTutari;

                kredi.KalanBorc -= dto.TahsilatTutari;

                var log = new Tahsilat
                {
                    KrediId = dto.KrediId,
                    HesapId = dto.HesapId,
                    Kredi = null!,
                    Hesap = null!,
                    TahsilatTutari = dto.TahsilatTutari,
                    IslemTarihi = DateTime.UtcNow
                };
                _context.T_Tahsilatlar.Add(log); 

                await _context.SaveChangesAsync(); 

                await transaction.CommitAsync();

                return Ok(new { Message = "Tahsilat başarıyla tamamlandı.", YeniKalanBorc = kredi.KalanBorc });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, "İşlem sırasında bir hata oluştu: " + ex.Message); 
            }
        }
    }
}