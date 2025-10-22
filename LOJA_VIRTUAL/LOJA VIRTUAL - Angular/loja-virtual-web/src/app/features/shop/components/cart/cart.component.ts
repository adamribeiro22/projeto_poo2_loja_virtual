import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { CartService } from '../../services/cart.service';
import { Carrinho } from '../../../../core/models/cart.model';

@Component({
  selector: 'app-cart',
  standalone: false,
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.scss']
})
export class CartComponent implements OnInit {
  cart$!: Observable<Carrinho>;

  constructor(private cartService: CartService) { }

  ngOnInit(): void {
    this.cart$ = this.cartService.cart$;
  }
  
  removerItem(variacaoId: number): void {
    this.cartService.removerItem(variacaoId);
  }
  
  finalizarCompra(): void {
    this.cartService.finalizarCompra().subscribe({
      next: () => {
      },
      error: (err) => {
        alert(`Ocorreu um erro ao finalizar a compra: ${err.message}`);
        console.error(err);
      }
    });
  }
}