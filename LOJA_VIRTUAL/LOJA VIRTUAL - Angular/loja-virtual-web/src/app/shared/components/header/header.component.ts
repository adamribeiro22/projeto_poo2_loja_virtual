import { Component, Input } from '@angular/core';
import { AuthService } from '../../../core/services/auth.service';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { NavLink } from '../../../core/models/NavLink';
import { Usuario } from '../../../core/models/usuario.model';
import { CartService } from '../../../features/shop/services/cart.service';

@Component({
  selector: 'app-header',
  standalone: false,
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent {
  @Input() navLinks: NavLink[] = [];
  @Input() title: string = 'App';
  @Input() titleLink: string = '/';

  totalItensCarrinho$: Observable<number>;
  usuarioAtual$: Observable<Usuario | null>;
  
  constructor(
    private authService: AuthService,
    private cartService: CartService
  ) {
    this.usuarioAtual$ = this.authService.usuarioAtual$;
    this.totalItensCarrinho$ = this.cartService.cart$.pipe(map(cart => cart.totalItens));
  }

  logout(): void {
    this.authService.logout();
  }
}