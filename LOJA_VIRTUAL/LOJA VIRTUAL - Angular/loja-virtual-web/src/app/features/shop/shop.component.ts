import { Component } from '@angular/core';
import { NavLink } from '../../core/models/NavLink';

@Component({
  selector: 'app-shop',
  standalone: false,
  templateUrl: './shop.component.html',
  styleUrl: './shop.component.scss'
})
export class ShopComponent {
  shopNavLinks: NavLink[] = [
    { path: 'produtos', label: 'Produtos' },
    { path: 'carrinho', label: 'Carrinho' },
    { path: 'meus-pedidos', label: 'Meus Pedidos' }
  ]
}
