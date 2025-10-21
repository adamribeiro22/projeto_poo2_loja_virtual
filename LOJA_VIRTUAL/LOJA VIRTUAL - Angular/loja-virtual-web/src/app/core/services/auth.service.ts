import { Injectable } from '@angular/core';import { HttpClient } from '@angular/common/http';
import { Route, Router } from '@angular/router';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { environment } from '../../../environments/environment';
import { AuthResponse } from '../models/auth-response.model';
import { Usuario } from '../models/usuario.model';

@Injectable({
  providedIn: 'root'
})

export class AuthService {
  private apiUrl = environment.apiUrl + '/api/Auth';
  private usuarioAtualSubject = new BehaviorSubject<Usuario | null>(null);
  public usuarioAtual$ = this.usuarioAtualSubject.asObservable();

  constructor(private http: HttpClient, private router: Router) {
    this.carregarUsuarioLogado();
   }

  login(credenciais: any): Observable<AuthResponse>{
    return this.http.post<AuthResponse>(`${this.apiUrl}/login`, credenciais).pipe(
      tap(response => {
        if (response.sucesso && response.token && response.usuario){
          this.guardarSessao(response.token, response.usuario);
          this.redirecionarAposLogin(response.usuario.tipo);
        }
      })
    )
  }

  register(dadosRegistro: any): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.apiUrl}/register`, dadosRegistro).pipe(
      tap(response => {
        if (response.sucesso && response.token && response.usuario) {
          this.guardarSessao(response.token, response.usuario);
          this.redirecionarAposLogin('Comum');
        }
      })
    );
  }

  logout(): void {
    localStorage.removeItem('jwt_token');
    localStorage.removeItem('usuario_atual');
    this.usuarioAtualSubject.next(null);
    this.router.navigate(['/login']);
  }

  public get usuarioLogado(): Usuario | null {
    return this.usuarioAtualSubject.value;
  }

  public get token(): string | null {
    return localStorage.getItem('jwt_token');
  }

  private guardarSessao(token: string, usuario: Usuario) : void {
    localStorage.setItem('jwt_token', token);
    localStorage.setItem('usuario_atual', JSON.stringify(usuario));
    this.usuarioAtualSubject.next(usuario);
  }

  private carregarUsuarioLogado(): void {
    const usuarioJson = localStorage.getItem('usuario_atual');
    if (usuarioJson) {
      const usuario = JSON.parse(usuarioJson) as Usuario;
      this.usuarioAtualSubject.next(usuario);
    }
  }

  private redirecionarAposLogin(tipo: 'Admin' | 'Comum'): void {
    if (tipo == 'Admin'){
      this.router.navigate(['/admin'])
    } else {
      this.router.navigate(['/shop'])
    }
  }
}
