using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Tahsilat
{
    [Key]
    public int TahsilatId { get; set; }

    public int KrediId { get; set; }
    [ForeignKey("KrediId")]
    public required KrediHesap Kredi { get; set; }

    public int HesapId { get; set; }
    [ForeignKey("HesapId")]
    public required MevduatHesap Hesap { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal TahsilatTutari { get; set; }

    public DateTime IslemTarihi { get; set; } = DateTime.Now;
}