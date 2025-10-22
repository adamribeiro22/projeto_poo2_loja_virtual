import { CartService } from '../../services/cart.service';
import { Produto, VariacaoProduto } from '../../../../core/models/produto.model';
import { Component, Input, OnInit, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';

@Component({
  selector: 'app-product-card',
  standalone: false,
  templateUrl: './product-card.component.html',
  styleUrls: ['./product-card.component.scss']
})
export class ProductCardComponent implements OnInit, OnDestroy {
  @Input() produto!: Produto;
  
  selecaoForm: FormGroup;
  variacaoSelecionada: VariacaoProduto | null = null;
  maxQuantidadePermitida = 1;

  private destroy$ = new Subject<void>();
  
  constructor(private fb: FormBuilder, private cartService: CartService) {
    this.selecaoForm = this.fb.group({
      variacaoId: [null, Validators.required],
      quantidade: [1, [Validators.required, Validators.min(1)]]
    });
  }

  ngOnInit(): void {
    this.selecaoForm.get('variacaoId')?.valueChanges
      .pipe(takeUntil(this.destroy$))
      .subscribe(id => {
        this.variacaoSelecionada = this.produto.variacoes.find(v => v.id === +id) || null;
        this.atualizarMaxQuantidade();
    });

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
    if (!this.variacaoSelecionada || !this.variacaoSelecionada.estoque) {
      this.maxQuantidadePermitida = 0;
    } else {
      const itemNoCarrinho = this.cartService.getItemAtualDoCarrinho(this.variacaoSelecionada.id);
      const quantidadeNoCarrinho = itemNoCarrinho ? itemNoCarrinho.quantidade : 0;
      this.maxQuantidadePermitida = this.variacaoSelecionada.estoque.quantidade - quantidadeNoCarrinho;
    }
    
    this.selecaoForm.get('quantidade')?.setValidators([
        Validators.required, 
        Validators.min(1), 
        Validators.max(this.maxQuantidadePermitida)
    ]);
    this.selecaoForm.get('quantidade')?.updateValueAndValidity();
  }

  adicionarAoCarrinho(): void {
    if (this.selecaoForm.invalid || !this.variacaoSelecionada) {
      return;
    }
    
    const { quantidade } = this.selecaoForm.value;
    this.cartService.adicionarAoCarrinho(this.produto, this.variacaoSelecionada, quantidade);
  }
  
  get estoqueDisponivel(): boolean {
    return this.maxQuantidadePermitida > 0;
  }
}