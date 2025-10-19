namespace LojaVirtual.Domain.Filters
{
    public class VendaFilter
    {
        public int? UsuarioId { get; set; }
        public DateTime? DataVendaDe { get; set; }
        public DateTime? DataVendaAte { get; set; }
        public decimal? ValorTotalMinimo { get; set; }
        public decimal? ValorTotalMaximo { get; set; }
    }
}
