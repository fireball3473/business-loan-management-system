using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class MevduatHesap
{
    [Key]
    public int HesapId { get; set; }

    public int MusteriId { get; set; }
    [ForeignKey("MusteriId")]
    public required Musteri Musteri { get; set; }

    [Required, StringLength(34)]
    public required string IbanNo { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Bakiye { get; set; } = 0;

    [Required, StringLength(3)]
    public required string DovizCinsi { get; set; }
}