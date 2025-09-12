import { inject } from '@angular/core';
import { CanActivateFn } from '@angular/router';
import { ToastService } from '../services/toast-service';
import { AccountService } from '../services/account-service';

export const authGuard: CanActivateFn = () => {
  const accountService = inject(AccountService);
  const toast = inject(ToastService);

  if (!accountService.currentUser()) {
    toast.error('Musisz być zalogowany, aby uzyskać dostęp do tej strony');
    return false;
  }
  return true;
};
