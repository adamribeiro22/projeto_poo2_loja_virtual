import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged, switchMap, takeUntil } from 'rxjs/operators';
import { AdminService } from '../../services/admin.service';
import { Venda } from '../../../../core/models/venda.model';
import { StatusVenda } from '../../../../core/models/status-venda.enum';
import { VendaQuery } from '../../../../core/models/venda-query.model';
import { FormBuilder, FormGroup } from '@angular/forms';

@Component({
  selector: 'app-order-list',
  standalone: false,
  templateUrl: './order-list.component.html',
  styleUrls: ['./order-list.component.scss']
})
export class OrderListComponent implements OnInit, OnDestroy {

  filtroForm!: FormGroup;
  private todasAsVendas: Venda[] = [];
  vendasDaPagina: Venda[] = [];

  currentPage = 1;
  pageSize = 10;
  totalCount = 0;

  isLoading = true;
  StatusVendaEnum = StatusVenda;
  private destroy$ = new Subject<void>();

  constructor(
    private adminService: AdminService,
    private router: Router,
    private fb: FormBuilder
  ) { }

  ngOnInit(): void {
    this.filtroForm = this.fb.group({
      usuarioId: [null],
      dataVendaDe: [null],
      dataVendaAte: [null],
      valorTotalMinimo: [null],
      valorTotalMaximo: [null]
    });

    this.filtroForm.valueChanges.pipe(
      debounceTime(500),
      distinctUntilChanged(),
      switchMap((filtros: VendaQuery) => {
        this.isLoading = true;
        return this.adminService.getVendas(filtros);
      }),
      takeUntil(this.destroy$)
    ).subscribe((vendas: Venda[]) => {
      this.todasAsVendas = vendas;
      this.totalCount = vendas.length;
      this.currentPage = 1;
      this.atualizarPaginaExibida();
      this.isLoading = false;
    });

    this.carregarVendas();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  carregarVendas(): void {
    this.filtroForm.updateValueAndValidity({ onlySelf: true, emitEvent: true });
  }

  atualizarPaginaExibida(): void {
    const startIndex = (this.currentPage - 1) * this.pageSize;
    const endIndex = startIndex + this.pageSize;
    this.vendasDaPagina = this.todasAsVendas.slice(startIndex, endIndex);
  }

  verDetalhes(vendaId: number): void {
    this.router.navigate(['/admin/pedidos', vendaId]);
  }

  atualizarStatus(vendaId: number, novoStatus: StatusVenda): void {
    const statusString = StatusVenda[novoStatus];

    this.adminService.updateStatusVenda(vendaId, statusString)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: () => {
          this.carregarVendas();
        },
        error: (err) => console.error('Falha ao atualizar status', err)
      });
  }

  isAnimando(venda: Venda): boolean {
    return (venda as any).animandoEnvio === true;
  }

  animarCaminhao(venda: Venda): void {
    (venda as any).animandoEnvio = true;
    setTimeout(() => {
      this.atualizarStatus(venda.id, this.StatusVendaEnum.Enviada);
    }, 1000);
  }

  limparFiltros(): void {
    this.filtroForm.reset({
      usuarioId: null, dataVendaDe: null, dataVendaAte: null,
      valorTotalMinimo: null, valorTotalMaximo: null
    });
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

  getStatusNome(status: StatusVenda): string {
    return StatusVenda[status] || 'Desconhecido';
  }
}