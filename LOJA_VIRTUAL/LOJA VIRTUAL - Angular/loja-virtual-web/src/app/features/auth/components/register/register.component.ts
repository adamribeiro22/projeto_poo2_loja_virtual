import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ValidationErrors, Validators } from '@angular/forms';
import { AuthService } from '../../../../core/services/auth.service';
import { finalize } from 'rxjs';

@Component({
  selector: 'app-register',
  standalone: false,
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss'
})
export class RegisterComponent implements OnInit {
  registerForm!: FormGroup;
  errorMessage: string | null = null;
  isLoading = false;

  constructor(private fb: FormBuilder, private authService: AuthService) { }

  ngOnInit(): void {
    this.registerForm = this.fb.group({
      nome: ['', [Validators.required, Validators.minLength(3)]],
      email: ['', [Validators.required, Validators.email]],
      senha: ['', [Validators.required, Validators.minLength(8)]],
      confirmarSenha: ['', [Validators.required]]
    }, {
      validators: this.passwordMatchValidator  
    });
  }

  passwordMatchValidator(control: AbstractControl): ValidationErrors | null {
    const senha = control.get('senha');
    const confirmarSenha = control.get('confirmarSenha');

    if (!senha || !confirmarSenha) {
      return null;
    }
    
    if (confirmarSenha.pristine) {
        return null;
    }

    if (senha.value === confirmarSenha.value) {
      if (confirmarSenha.hasError('mismatch')) {
          const errors = confirmarSenha.errors;
          if (errors) {
              delete errors['mismatch'];
              if (Object.keys(errors).length === 0) {
                  confirmarSenha.setErrors(null);
              } else {
                  confirmarSenha.setErrors(errors);
              }
          }
      }
      return null;
    } else {
      confirmarSenha.setErrors({ ...confirmarSenha.errors, mismatch: true });
      return { mismatch: true }; 
    }
  }

  onSubmit(): void {
    if (this.registerForm.invalid) {
      this.registerForm.markAllAsTouched(); 
      return;
    }

    this.isLoading = true;
    this.errorMessage = null;
    
    const { confirmarSenha, ...formData } = this.registerForm.value;
    console.log(formData);
    this.authService.register(formData).pipe(
      finalize(() => this.isLoading = false)
    ).subscribe({
      next: (response) => {
      },
      error: (err) => {
        this.errorMessage = err.error?.mensagem || 'Ocorreu um erro ao tentar efetuar o registo.';
      }
    });
  }
}
