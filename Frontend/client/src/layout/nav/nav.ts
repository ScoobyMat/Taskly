
import { Component, inject, signal } from '@angular/core';
import { AccountService } from '../../core/services/account-service';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { ToastService } from '../../core/services/toast-service';

@Component({
  selector: 'app-nav',
  imports: [RouterLink, RouterLinkActive],
  templateUrl: './nav.html',
  styleUrl: './nav.css'
})
export class Nav {
protected accountService = inject(AccountService);
private router = inject(Router);
private toast = inject(ToastService);
private creds: any = {};

login() {
  this.accountService.login(this.creds).subscribe({
    next: () => {
      this.router.navigateByUrl('/tasks');
      this.toast.success('Zalogowano pomyślnie');
      this.creds = {};
    },
    error: error => {
      this.toast.error(error.message);
    }
  })
}

logout() {
  this.accountService.logout();
  this.router.navigateByUrl('/');
  this.toast.success('Wylogowano pomyślnie');
}
}
