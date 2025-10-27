import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { map, take } from 'rxjs/operators';

export const adminGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  return authService.usuarioAtual$.pipe(
    take(1), map(usuario => {
      if(usuario && usuario.tipo === 'Admin'){
        return true;
      }

      if(usuario && usuario.tipo === 'Comum'){
        router.navigate(['/shop']);
        return false;
      }

      router.navigate(['/auth/login']);
      return false;
    })
  )
};
