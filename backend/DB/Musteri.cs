using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Musteri
{
    [Key]
    public int MusteriId { get; set; }

    [Required, StringLength(10)]
    public required string VKN { get; set; }

    [Required, StringLength(255)]
    public required string Unvan { get; set; }

    public required string Sektor { get; set; }

    public required ICollection<MevduatHesap> MevduatHesaplari { get; set; }
    public required ICollection<KrediHesap> Krediler { get; set; }
}