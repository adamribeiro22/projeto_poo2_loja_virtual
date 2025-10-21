import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable, switchMap } from 'rxjs';
import { Venda } from '../../../../core/models/venda.model';
import { AdminService } from '../../services/admin.service';
import { StatusVenda } from '../../../../core/models/status-venda.enum';

@Component({
  selector: 'app-order-detail',
  standalone: false,
  templateUrl: './order-detail.component.html',
  styleUrl: './order-detail.component.scss'
})
export class OrderDetailComponent implements OnInit {
  venda$!: Observable<Venda>;

  constructor(
    private route: ActivatedRoute,
    private adminService: AdminService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.venda$ = this.route.paramMap.pipe(
      switchMap(params => {
        const id = Number(params.get('id'));
        return this.adminService.getVendaById(id);
      })
    );
  }

  getStatusNome(status: StatusVenda): string {
    return StatusVenda[status];
  }

  voltarParaLista(): void {
    this.router.navigate(['/admin/pedidos']);
  }
}