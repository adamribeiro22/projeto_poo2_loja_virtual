namespace LojaVirtual.Application.DTO.Query
{
    public class VendaQueryDTO
    {
        public int? UsuarioId { get; set; }
        public DateTime? DataVendaDe { get; set; }
        public DateTime? DataVendaAte { get; set; }
        public decimal? ValorTotalMinimo { get; set; }
        public decimal? ValorTotalMaximo { get; set; }
    }
}
