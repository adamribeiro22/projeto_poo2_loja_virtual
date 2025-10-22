import { Produto, VariacaoProduto } from './produto.model';

export interface ItemCarrinho {
  produto: Produto;
  variacao: VariacaoProduto;
  quantidade: number;
}

export interface Carrinho {
  itens: ItemCarrinho[];
  valorTotal: number;
  totalItens: number;
}