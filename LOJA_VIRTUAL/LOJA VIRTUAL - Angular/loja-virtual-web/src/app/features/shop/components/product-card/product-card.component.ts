import { CartService } from '../../services/cart.service';
import { Produto, VariacaoProduto } from '../../../../core/models/produto.model';
import { Component, Input, OnInit, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { environment } from '../../../../../environments/environment.development';

@Component({
  selector: 'app-product-card',
  standalone: false,
  templateUrl: './product-card.component.html',
  styleUrls: ['./product-card.component.scss']
})
export class ProductCardComponent implements OnInit, OnDestroy {
  @Input() produto!: Produto;
  @Input() variacao!: VariacaoProduto; 
  
  selecaoForm: FormGroup;
  maxQuantidadePermitida = 1;

  public apiUrl = environment.apiUrl;
  private placeholderImg = '../../../../../assets/carrinho.jpg';

  private destroy$ = new Subject<void>();
  
  constructor(private fb: FormBuilder, private cartService: CartService) {
    this.selecaoForm = this.fb.group({
      quantidade: [1, [Validators.required, Validators.min(1)]]
    });
  }

  ngOnInit(): void {
    this.atualizarMaxQuantidade();
    this.cartService.cart$
      .pipe(takeUntil(this.destroy$))
      .subscribe(() => {
        this.atualizarMaxQuantidade();
      });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
  
  private atualizarMaxQuantidade(): void {
    if (!this.variacao || !this.variacao.estoque) {
      this.maxQuantidadePermitida = 0;
    } else {
      const itemNoCarrinho = this.cartService.getItemAtualDoCarrinho(this.variacao.id);
      const quantidadeNoCarrinho = itemNoCarrinho ? itemNoCarrinho.quantidade : 0;
      this.maxQuantidadePermitida = this.variacao.estoque.quantidade - quantidadeNoCarrinho;
    }
    
    this.selecaoForm.get('quantidade')?.setValidators([
        Validators.required, 
        Validators.min(1), 
        Validators.max(this.maxQuantidadePermitida)
    ]);
    this.selecaoForm.get('quantidade')?.updateValueAndValidity();
  }

  adicionarAoCarrinho(): void {
    if (this.selecaoForm.invalid) {
      return;
    }
    
    const { quantidade } = this.selecaoForm.value;
    this.cartService.adicionarAoCarrinho(this.produto, this.variacao, quantidade);
  }
  
  get estoqueDisponivel(): boolean {
    return this.maxQuantidadePermitida > 0;
  }

  onImageError(event: Event): void {
    const element = event.target as HTMLImageElement;
    element.src = this.placeholderImg;
  }

  onAddToCart(event: MouseEvent) {
  const button = event.currentTarget as HTMLButtonElement;

  if (button.disabled) {
    button.classList.add('shake');
    setTimeout(() => button.classList.remove('shake'), 400);
    return;
  }

  this.adicionarAoCarrinho();

  button.classList.add('pulse');
  setTimeout(() => button.classList.remove('pulse'), 600);
}
}
