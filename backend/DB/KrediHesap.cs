using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class KrediHesap // Kredi hesap tablosunun yapısını tanıtır. 
{
    // Get set özelliklere kolayca erişmeni ve değer atayabilmeyi sağlar.
    [Key]
    public int KrediId { get; set; }

    public int MusteriId { get; set; }
    [ForeignKey("MusteriId")]
    public required Musteri Musteri { get; set; }

    public required string KrediTuru { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal AnaPara { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal ToplamBorc { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal KalanBorc { get; set; }

    public decimal FaizOrani { get; set; }
    public int VadeSayisi { get; set; }
    public required string DovizCinsi { get; set; }
    public required string OdemeTipi { get; set; }
    public DateTime BaslangicTarihi { get; set; } = DateTime.Now;
}