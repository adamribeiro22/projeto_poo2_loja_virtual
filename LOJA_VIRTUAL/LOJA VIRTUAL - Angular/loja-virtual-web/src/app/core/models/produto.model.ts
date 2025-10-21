export interface Estoque {
  id: number;
  quantidade: number;
}

export interface VariacaoProduto {
  id: number;
  produtoId: number;
  tamanho?: string;
  cor?: string;
  preco: number;
  ativo: boolean;
  estoque?: Estoque;
}

export interface Produto {
  id: number;
  nome: string;
  descricao?: string;
  categoria?: string;
  ativo: boolean;
  variacoes: VariacaoProduto[];
}

export interface VariacaoProdutoCreateDTO {
  produtoId: number;
  tamanho?: string;
  cor?: string;
  preco: number;
  quantidadeEstoqueInicial: number;
}

export interface ProdutoCreateDTO {
  nome: string;
  descricao?: string;
  categoria?: string;
  variacoes: VariacaoProdutoCreateDTO[];
}