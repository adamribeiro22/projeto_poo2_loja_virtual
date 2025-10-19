import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AuthRoutingModule } from './auth-routing.module';
import { AuthComponent } from './auth.component';
import { LoginComponent } from './components/login/login.component';
import { ReactiveFormsModule } from '@angular/forms';

@NgModule({
  declarations: [
    // Permite o angular entender como esses componentes são construídos e que eles estão disponíveis, mas não significa que eles estão carregados.
    AuthComponent,
    LoginComponent  
  ],
  imports: [
    // Importa novas regras, incluindo as novas rotas, que a partir de agora o "AuthRoutingModule" que vai gerir
    CommonModule,
    AuthRoutingModule,
    ReactiveFormsModule 
  ]
})

export class AuthModule { }