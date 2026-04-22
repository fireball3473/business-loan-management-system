using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Kullanici
{
    [Key]
    public int KullaniciId { get; set; }

    [Required, StringLength(50)]
    public required string KullaniciAdi { get; set; }

    [Required]
    public required string Sifre { get; set; }

    public required string AdSoyad { get; set; }
}