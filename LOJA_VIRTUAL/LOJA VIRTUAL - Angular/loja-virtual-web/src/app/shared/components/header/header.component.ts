import { Component, Input } from '@angular/core';
import { AuthService } from '../../../core/services/auth.service';
import { combineLatest, Observable } from 'rxjs';
import { filter, map, startWith } from 'rxjs/operators';
import { NavLink } from '../../../core/models/NavLink';
import { Usuario } from '../../../core/models/usuario.model';
import { CartService } from '../../../features/shop/services/cart.service';
import { NavigationEnd, Router } from '@angular/router';

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
  showCart$!: Observable<boolean>;

  constructor(
    private authService: AuthService,
    private cartService: CartService,
    private router: Router
  ) {
    this.usuarioAtual$ = this.authService.usuarioAtual$;
    this.totalItensCarrinho$ = this.cartService.cart$.pipe(map(cart => cart.totalItens));
  }

  ngOnInit(): void {
    const currentUrl$ = this.router.events.pipe(
      filter((event): event is NavigationEnd => event instanceof NavigationEnd),
      map((event: NavigationEnd) => event.urlAfterRedirects),
      startWith(this.router.url)
    );

    this.showCart$ = combineLatest([
      this.usuarioAtual$,
      currentUrl$
    ]).pipe(
      map(([user, url]) => {
        const isUserRole = user && user.tipo === 'Comum';
        const isShopRoute = url.startsWith('/shop');
        return !!(isUserRole && isShopRoute);
      })
    );
  }

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/auth/login']); 
  }
}