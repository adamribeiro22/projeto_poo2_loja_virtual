import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { ProductService } from '../../services/product.service';
import { Produto } from '../../../../core/models/produto.model';

@Component({
  selector: 'app-product-list',
  standalone: false,
  templateUrl: './product-list.component.html',
  styleUrls: ['./product-list.component.scss']
})
export class ProductListComponent implements OnInit, OnDestroy {
  
  private todosOsProdutos: Produto[] = [];
  produtosDaPagina: Produto[] = [];
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
          this.todosOsProdutos = produtos;
          this.totalCount = produtos.length;
          this.atualizarPaginaExibida();
          this.isLoading = false;
        },
        error: (err) => {
          console.error("Falha ao carregar produtos", err);
          this.isLoading = false;
        }
      });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  atualizarPaginaExibida(): void {
    const startIndex = (this.currentPage - 1) * this.pageSize;
    const endIndex = startIndex + this.pageSize;
    this.produtosDaPagina = this.todosOsProdutos.slice(startIndex, endIndex);
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