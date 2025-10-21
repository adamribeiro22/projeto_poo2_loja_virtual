import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../../../../core/services/auth.service';
import { finalize } from 'rxjs';

@Component({
  selector: 'app-login',
  standalone: false,  
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent implements OnInit {
  // FormGroup (formulário), FormBuilder (serviço do forms), Validators (funções de validações) todos vem do ReactiveFormsModule
  loginForm!: FormGroup;
  errorMessage: string | null = null;
  isLoading = false;

  constructor( private fb: FormBuilder, private authService: AuthService ) { }

  // Um dos ciclos de vida do Angular que é muito usado para essa configuração de validação inicial do form
  ngOnInit(): void {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      senha: ['', [Validators.required]]
    });
  }

  onSubmit(): void {
    if (this.loginForm.invalid) {
      return;
    }

    this.isLoading = true;
    this.errorMessage = null;

    // Pipe() permite lógicas extras sobre a requisição. Subscribe trata o Observable do service (next para sucesso, e error pra erro de requisição).
    this.authService.login(this.loginForm.value).pipe(
      finalize(() => this.isLoading = false)
    ).subscribe({
      next: (response) => {
      },
      error: (err) => {
        this.errorMessage = err.error?.mensagem || 'Ocorreu um erro ao tentar fazer login.';
      }
    });
  }
}
