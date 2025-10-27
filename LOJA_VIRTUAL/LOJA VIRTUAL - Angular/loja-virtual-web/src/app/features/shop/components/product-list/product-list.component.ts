import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { ProductService } from '../../services/product.service';
import { Produto, VariacaoProduto } from '../../../../core/models/produto.model';

interface ItemExibicao {
  produto: Produto;
  variacao: VariacaoProduto;
}

@Component({
  selector: 'app-product-list',
  standalone: false,
  templateUrl: './product-list.component.html',
  styleUrls: ['./product-list.component.scss']
})
export class ProductListComponent implements OnInit, OnDestroy {
  
  private todosOsItens: ItemExibicao[] = [];
  itensDaPagina: ItemExibicao[] = [];
  isLoading = true;

  currentPage = 1;
  pageSize = 10;
  totalCount = 0;

  private destroy$ = new Subject<void>();

  constructor(private productService: ProductService) { }

  ngOnInit(): void {
    this.productService.getProdutosAtivos()
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (produtos) => {
          this.todosOsItens = this.mapearProdutosParaItens(produtos);
          this.totalCount = this.todosOsItens.length;
          this.atualizarPaginaExibida();
          this.isLoading = false;
        },
        error: (err) => {
          console.error("Falha ao carregar produtos", err);
          this.isLoading = false;
        }
      });
  }

  private mapearProdutosParaItens(produtos: Produto[]): ItemExibicao[] {
    return produtos.flatMap(produto => 
      produto.variacoes
        .filter(v => v.ativo)
        .map(variacao => ({
          produto: produto,
          variacao: variacao
        }))
    );
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  atualizarPaginaExibida(): void {
    const startIndex = (this.currentPage - 1) * this.pageSize;
    const endIndex = startIndex + this.pageSize;
    this.itensDaPagina = this.todosOsItens.slice(startIndex, endIndex);
  }

  get totalPages(): number {
    return Math.ceil(this.totalCount / this.pageSize);
  }

  mudarPagina(novaPagina: number): void {
    if (novaPagina > 0 && novaPagina <= this.totalPages) {
      this.currentPage = novaPagina;
      this.atualizarPaginaExibida();
    }
  }
}
