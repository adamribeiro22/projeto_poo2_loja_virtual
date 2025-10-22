export interface ItemVendaCreateDTO {
  variacaoProdutoId: number;
  quantidade: number;
}

export interface VendaCreateDTO {
  usuarioId: number;
  itens: ItemVendaCreateDTO[];
}