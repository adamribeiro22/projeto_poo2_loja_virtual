import { Component } from '@angular/core';
import { NavLink } from '../../core/models/NavLink';

@Component({
  selector: 'app-admin',
  standalone: false,
  templateUrl: './admin.component.html',
  styleUrl: './admin.component.scss'
})
export class AdminComponent {
  adminNavLinks: NavLink[] = [
    { path: 'pedidos', label: 'Pedidos' },
    { path: 'produtos', label: 'Produtos' }
  ];
}