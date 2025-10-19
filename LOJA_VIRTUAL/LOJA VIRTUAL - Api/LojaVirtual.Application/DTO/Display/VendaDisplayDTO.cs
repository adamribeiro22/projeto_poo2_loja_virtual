namespace LojaVirtual.Application.DTO.Display
{
    public class VendaDisplayDTO
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public string NomeUsuario { get; set; }
        public DateTime DataVenda { get; set; }
        public decimal ValorTotal { get; set; }
        public string Status { get; set; }

        public List<ItemVendaDisplayDTO> Itens { get; set; }
    }
}
