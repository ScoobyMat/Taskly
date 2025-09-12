import { inject } from '@angular/core';
import { CanActivateFn } from '@angular/router';
import { ToastService } from '../services/toast-service';
import { AuthService } from '../services/auth-service';

export const authGuard: CanActivateFn = () => {
  const authService = inject(AuthService);
  const toast = inject(ToastService);

  if (!authService.currentUser()) {
    toast.error('Musisz być zalogowany, aby uzyskać dostęp do tej strony');
    return false;
  }
  return true;
};
