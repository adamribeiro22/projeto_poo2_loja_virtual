import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { AdminService } from '../../services/admin.service';
import { Venda } from '../../../../core/models/venda.model';
import { StatusVenda } from '../../../../core/models/status-venda.enum';

@Component({
  selector: 'app-order-list',
  standalone: false,
  templateUrl: './order-list.component.html',
  styleUrls: ['./order-list.component.scss']
})
export class OrderListComponent implements OnInit, OnDestroy {
  vendas: Venda[] = [];
  isLoading = true;
  StatusVendaEnum = StatusVenda;
  private destroy$ = new Subject<void>();

  constructor(
    private adminService: AdminService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.carregarVendas();
  }

  carregarVendas(): void {
    this.isLoading = true;
    this.adminService.getVendas()
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (vendas) => {
          this.vendas = vendas;
          this.isLoading = false;
        },
        error: () => this.isLoading = false
      });
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
        error: (err) => {
          console.error('Falha ao atualizar status', err);
        }
      });
  }

  getStatusNome(status: StatusVenda): string {
    return StatusVenda[status];
  }

  isAnimando(venda: Venda): boolean {
    return (venda as any).animandoEnvio === true;
  }

  animarCaminhao(venda: Venda): void {
    (venda as any).animandoEnvio = true;

    setTimeout(() => {
      this.atualizarStatus(venda.id, this.StatusVendaEnum.Enviada);
      (venda as any).animandoEnvio = false;
    }, 1000);
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}