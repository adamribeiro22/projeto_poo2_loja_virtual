namespace LojaVirtual.Application.DTO.Creation
{
    public class VendaCreateDTO
    {
        public int UsuarioId { get; set; }
        public List<ItemVendaCreateDTO> Itens { get; set; }
    }
}
