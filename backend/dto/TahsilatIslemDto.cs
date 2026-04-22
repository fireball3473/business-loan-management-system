/// <summary>
/// Kredi ödemesi (tahsilat) sırasında hangi hesaptan ne kadar para çekileceğini taşıyan veri nesnesi.
/// </summary>
public class TahsilatIslemDto
{
    public int KrediId { get; set; }
    public int HesapId { get; set; }
    public decimal TahsilatTutari { get; set; }
}