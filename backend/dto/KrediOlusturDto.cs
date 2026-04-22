/// <summary>
/// Kullanıcının kredi formunda doldurduğu verileri backend'e taşımak için kullanılan Data Transfer Object.
/// </summary>
public class KrediOlusturDto
{
    public required string VKN { get; set; }
    public required string KrediTuru { get; set; }
    public decimal AnaPara { get; set; }
    public required string DovizCinsi { get; set; }
    public required string OdemeTipi { get; set; }
    public int VadeSayisi { get; set; }
    public decimal FaizOrani { get; set; }
}