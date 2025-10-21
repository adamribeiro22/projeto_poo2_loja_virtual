import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { VendaQuery } from '../../../core/models/venda-query.model';
import { Venda } from '../../../core/models/venda.model';
import { StatusVenda  } from '../../../core/models/status-venda.enum';
import { Produto, ProdutoCreateDTO, VariacaoProdutoCreateDTO } from '../../../core/models/produto.model';
import { ProdutoQuery } from '../../../core/models/produto-query.model';

@Injectable({
  providedIn: 'root'
})
export class AdminService {
  private apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getVendas(query?: VendaQuery): Observable<Venda[]> {
    let params = new HttpParams();
    if(query){
      if (query.usuarioId) 
        params = params.append('UsuarioId', query.usuarioId.toString());
      if (query.dataVendaDe) 
        params = params.append('DataVendaDe', query.dataVendaDe);
      if (query.dataVendaAte) 
        params = params.append('DataVendaAte', query.dataVendaAte);
      if (query.valorTotalMinimo) 
        params = params.append('ValorTotalMinimo', query.valorTotalMinimo.toString());
      if (query.valorTotalMaximo) 
        params = params.append('ValorTotalMaximo', query.valorTotalMaximo.toString());
    }

    return this.http.get<any[]>(`${this.apiUrl}/api/Vendas`, { params }).pipe(
      map(vendasApi => vendasApi.map(venda => this.mapVendaApiToModel(venda)))
    );
  }

  getVendaById(id: number): Observable<Venda> {
    return this.http.get<any>(`${this.apiUrl}/api/Vendas/${id}`).pipe(
      map(vendaApi => this.mapVendaApiToModel(vendaApi))
    );
  }
  
  updateStatusVenda(vendaId: number, novoStatus: string): Observable<void> {
    const dto = { novoStatus: novoStatus };
    return this.http.patch<void>(`${this.apiUrl}/api/Vendas/${vendaId}/status`, dto);
  }
  
  cancelVenda(vendaId: number): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/api/Vendas/${vendaId}/cancel`, {})
  }

  private mapVendaApiToModel(venda: any): Venda {
    return {
      ...venda,
      status: StatusVenda[venda.status as keyof typeof StatusVenda]
    };
  }

  getProdutos(query: ProdutoQuery): Observable<Produto[]> {
    let params = new HttpParams();
    if (query.nome) {
      params = params.append('Nome', query.nome);
    }
    if (query.categoria) {
      params = params.append('Categoria', query.categoria);
    }
    if (query.ativo !== null && query.ativo !== undefined) {
      params = params.append('Ativo', query.ativo.toString());
    }
    return this.http.get<Produto[]>(`${this.apiUrl}/api/Produtos`, { params });
  }

  getProdutoById(id: number): Observable<Produto> {
    return this.http.get<Produto>(`${this.apiUrl}/api/Produtos/${id}`);
  }

  createProduto(produtoData: ProdutoCreateDTO): Observable<Produto> {
    return this.http.post<Produto>(`${this.apiUrl}/api/Produtos`, produtoData);
  }

  updateProduto(id: number, produtoData: Partial<ProdutoCreateDTO>): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/api/Produtos/${id}`, produtoData);
  }
  
  cancelProduto(id: number): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/api/Produtos/${id}/cancel`, {});
  }

  reactivateProduto(id: number): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/api/Produtos/${id}/cancel`, {});
  }

  deleteProduto(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/api/Produtos/${id}`);
  }

  createVariacaoProduto(variacaoData: VariacaoProdutoCreateDTO): Observable<any> {
    return this.http.post(`${this.apiUrl}/api/VariacaoProdutos`, variacaoData);
  }

  updateVariacaoProduto(id: number, variacaoData: VariacaoProdutoCreateDTO): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/api/VariacaoProdutos/${id}`, variacaoData);
  }

  deleteVariacaoProduto(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/api/VariacaoProdutos/${id}`);
  }
}
