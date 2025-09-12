import { Component, inject, output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AccountService } from '../../../core/services/account-service';
import { LoginCreds } from '../../../shared/types/user';
import { Router } from '@angular/router';
import { ToastService } from '../../../core/services/toast-service';

@Component({
  selector: 'app-login',
  imports: [FormsModule],
  templateUrl: './login.html',
  styleUrl: './login.css',
})
export class Login {
  private accountService = inject(AccountService);
  private router = inject(Router);
  private toast = inject(ToastService);
  protected creds = {} as LoginCreds;
  cancelLogin = output<boolean>();

  login() {
    this.accountService.login(this.creds).subscribe({
      next: () => {
        this.toast.success('Zalogowano pomyślnie');
        this.router.navigateByUrl('/');
        this.cancel();
      },
      error: (error) => {
        this.toast.error(error.message);
      },
    });
  }

  cancel() {
    this.router.navigateByUrl('/');
  }
}
