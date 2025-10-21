namespace LojaVirtual.Application.DTO.Display
{
    public class ItemVendaDisplayDTO
    {
        public string NomeProduto { get; set; }
        public string? Tamanho { get; set; }
        public string? Cor { get; set; }
        public int Quantidade { get; set; }
        public decimal PrecoUnitario { get; set; }
        public decimal Subtotal => Quantidade * PrecoUnitario;
    }
}
