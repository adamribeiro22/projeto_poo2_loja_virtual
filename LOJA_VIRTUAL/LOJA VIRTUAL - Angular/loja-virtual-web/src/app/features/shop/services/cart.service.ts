import { Injectable } from '@angular/core';
import { Produto, VariacaoProduto } from '../../../core/models/produto.model';
import { Carrinho, ItemCarrinho } from '../../../core/models/cart.model';
import { AdminService } from '../../admin/services/admin.service';
import { AuthService } from '../../../core/services/auth.service';
import { BehaviorSubject, map, Observable, throwError } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CartService {
  private cartSubject = new BehaviorSubject<Carrinho>(this.getCartFromStorage());
  public cart$ = this.cartSubject.asObservable();
  private currentUserId: number | null = null;

  constructor(
    private adminService: AdminService,
    private authService: AuthService
  ) {
    this.authService.usuarioAtual$.subscribe(user => {
      this.currentUserId = user ? user.id : null;
    });
  }

  adicionarAoCarrinho(produto: Produto, variacao: VariacaoProduto, quantidade: number): void {
    if (!variacao.estoque) {
        console.error("Variação sem dados de estoque.");
        return;
    }

    const carrinhoAtual = this.cartSubject.value;
    const itemExistente = this.getItemAtualDoCarrinho(variacao.id);
    const estoqueDisponivel = variacao.estoque.quantidade;

    if (itemExistente) {
      const novaQuantidade = itemExistente.quantidade + quantidade;
      itemExistente.quantidade = Math.min(novaQuantidade, estoqueDisponivel);
    } else {
      const quantidadeAdicionar = Math.min(quantidade, estoqueDisponivel);
      if (quantidadeAdicionar > 0) {
        carrinhoAtual.itens.push({ produto, variacao, quantidade: quantidadeAdicionar });
      }
    }

    this.atualizarCarrinho(carrinhoAtual);
  }

  removerItem(variacaoId: number): void {
    const carrinhoAtual = this.cartSubject.value;
    carrinhoAtual.itens = carrinhoAtual.itens.filter(i => i.variacao.id !== variacaoId);
    this.atualizarCarrinho(carrinhoAtual);
  }

  finalizarCompra(): Observable<any> {
    if (!this.currentUserId) {
      return throwError(() => new Error("Utilizador não está logado para finalizar a compra."));
    }
    
    const cart = this.cartSubject.value;
    if (cart.itens.length === 0) {
      return throwError(() => new Error("O carrinho está vazio."));
    }

    const vendaDto = {
      usuarioId: this.currentUserId,
      itens: cart.itens.map(item => ({
        variacaoProdutoId: item.variacao.id,
        quantidade: item.quantidade
      }))
    };
    
    return this.adminService.createVenda(vendaDto).pipe(
      map(response => {
        this.limparCarrinho();
        return response;
      })
    );
  }
  
  limparCarrinho(): void {
    this.atualizarCarrinho({ itens: [], valorTotal: 0, totalItens: 0 });
  }

  public getItemAtualDoCarrinho(variacaoId: number): ItemCarrinho | undefined {
    return this.cartSubject.value.itens.find(i => i.variacao.id === variacaoId);
  }

  private atualizarCarrinho(carrinho: Carrinho): void {
    carrinho.totalItens = carrinho.itens.reduce((total, item) => total + item.quantidade, 0);
    carrinho.valorTotal = carrinho.itens.reduce((total, item) => total + (item.variacao.preco * item.quantidade), 0);
    
    localStorage.setItem('meu-carrinho', JSON.stringify(carrinho));
    this.cartSubject.next(carrinho);
  }
  
  private getCartFromStorage(): Carrinho {
      const carrinhoJson = localStorage.getItem('meu-carrinho');
      return carrinhoJson ? JSON.parse(carrinhoJson) : { itens: [], valorTotal: 0, totalItens: 0 };
  }
}