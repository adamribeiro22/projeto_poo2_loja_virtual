import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthComponent } from './auth.component';
import { LoginComponent } from './components/login/login.component';

const routes: Routes = [
  {
    path: '', // Corresponde a /auth
    component: AuthComponent, // Usa o AuthComponent como layout
    children: [
      {
        path: 'login', // Corresponde a /auth/login
        component: LoginComponent
      },
      {
        path: '', // Se alguém acessar a /auth, redireciona para /auth/login
        redirectTo: 'login',
        pathMatch: 'full'
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AuthRoutingModule { }