import { StatusVenda } from "./status-venda.enum";

export interface ItemVenda {
  nomeProduto?: string;
  tamanho?: string;
  cor?: string;
  quantidade: number;
  precoUnitario: number;
  subtotal: number;
}

export interface Venda {
  id: number;
  nomeUsuario: string;
  dataVenda: Date;
  valorTotal: number;
  status: StatusVenda;
  itens: ItemVenda[];
}