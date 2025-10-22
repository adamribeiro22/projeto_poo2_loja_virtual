import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { BehaviorSubject, debounceTime, distinctUntilChanged, Subject, switchMap, takeUntil, tap } from 'rxjs';
import { AdminService } from '../../services/admin.service';
import { Produto } from '../../../../core/models/produto.model';

@Component({
  selector: 'app-product-list',
  standalone: false,
  templateUrl: './product-list.component.html',
  styleUrls: ['./product-list.component.scss']
})
export class ProductListComponent implements OnInit, OnDestroy {
  
  filtroForm!: FormGroup;

  private todosOsProdutos: Produto[] = [];
  produtosDaPagina: Produto[] = [];

  currentPage = 1;
  pageSize = 10;
  totalCount = 0;

  isLoading = false;
  private destroy$ = new Subject<void>();
  private refreshTrigger$ = new BehaviorSubject<void>(undefined);

  constructor(
    private adminService: AdminService,
    private router: Router,
    private fb: FormBuilder
  ) { }

  ngOnInit(): void {
    this.filtroForm = this.fb.group({
      nome: [''],
      categoria: [''],
      ativo: [null]
    });

    this.filtroForm.valueChanges.pipe(
      debounceTime(500),
      distinctUntilChanged(),
      switchMap(filtros => {
        this.isLoading = true;
        return this.adminService.getProdutos(filtros);
      }),
      takeUntil(this.destroy$)
    ).subscribe(produtos => {
      this.todosOsProdutos = produtos;
      this.totalCount = produtos.length;
      this.currentPage = 1;
      this.atualizarPaginaExibida();
      this.isLoading = false;
    });

    this.carregarProdutos();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  carregarProdutos(): void {
    this.filtroForm.patchValue(this.filtroForm.value);
  }

  atualizarPaginaExibida(): void {
    const startIndex = (this.currentPage - 1) * this.pageSize;
    const endIndex = startIndex + this.pageSize;
    this.produtosDaPagina = this.todosOsProdutos.slice(startIndex, endIndex);
  }

  editarProduto(id: number): void {
    this.router.navigate(['/admin/produtos/editar', id]);
  }

  criarNovoProduto(): void {
    this.router.navigate(['/admin/produtos/novo']);
  }
  
  toggleAtivo(id: number, ativo: boolean): void {
    const action$ = ativo 
      ? this.adminService.cancelProduto(id) 
      : this.adminService.reactivateProduto(id);
      
    action$.subscribe(() => {
        this.carregarProdutos();
    });
  }

  limparFiltros(): void {
    this.filtroForm.reset({ ativo: null });
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