import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Produto } from '../../../core/models/produto.model';
import { environment } from '../../../../environments/environment';


@Injectable({
  providedIn: 'root'
})
export class ProductService {
  private apiUrl = `${environment.apiUrl}/api/Produtos/details`;

  constructor(private http: HttpClient) { }

  getProdutosAtivos(): Observable<Produto[]> {
    return this.http.get<Produto[]>(this.apiUrl, { params: { Ativo: 'true' } });
  }
}