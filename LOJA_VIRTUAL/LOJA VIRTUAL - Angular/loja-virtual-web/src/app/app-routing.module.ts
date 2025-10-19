import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  // Rota para o módulo de autenticação
  {
    path: 'auth',
    // loadChildren implementa a ideia do lazyLoading, aumentando a performance com carregamento feito apenas ao acessar essa rota  
    loadChildren: () => import('./features/auth/auth.module').then(m => m.AuthModule)
  },
  // Rota para o módulo da loja (shop)
  {
    path: 'shop',
    loadChildren: () => import('./features/shop/shop.module').then(m => m.ShopModule)
    // Futuramente, adicionaremos os guards aqui: canActivate: [AuthGuard]
  },
  // Rota para o módulo de administração (admin)
  {
    path: 'admin',
    loadChildren: () => import('./features/admin/admin.module').then(m => m.AdminModule)
    // Futuramente, adicionaremos os guards aqui: canActivate: [AuthGuard, AdminGuard]
  },
  // Rota padrão: redireciona para a tela de login
  {
    path: '',
    redirectTo: '/auth/login',
    pathMatch: 'full'
  },
  // Rota "catch-all": para qualquer URL não encontrada, redireciona para o login
  {
    path: '**',
    redirectTo: '/auth/login'
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})

export class AppRoutingModule { }