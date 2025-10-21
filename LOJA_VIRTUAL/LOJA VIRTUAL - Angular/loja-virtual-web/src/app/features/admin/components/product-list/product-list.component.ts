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
  
  produtos: Produto[] = [];
  filtroForm!: FormGroup;

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

    this.refreshTrigger$.pipe(
      tap(() => this.isLoading = true),
      switchMap(() => this.adminService.getProdutos(this.filtroForm.value)),
      takeUntil(this.destroy$)
    ).subscribe(result => {
      this.produtos = result;
      this.isLoading = false;
    });

    this.filtroForm.valueChanges.pipe(
      debounceTime(400),
      distinctUntilChanged((a, b) => JSON.stringify(a) === JSON.stringify(b)),
      takeUntil(this.destroy$)
    ).subscribe(() => {
      this.refreshTrigger$.next();
    });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  limparFiltros(): void {
    this.filtroForm.reset({
      nome: '',
      categoria: '',
      ativo: null
    });
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
        this.refreshTrigger$.next();
    });
  }
}