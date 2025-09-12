import { Component, inject, output } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { RegisterCreds } from '../../../shared/types/user';
import { AuthService } from '../../../core/services/auth-service';
import { Router } from '@angular/router';
import { ToastService } from '../../../core/services/toast-service';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './register.html',
  styleUrls: ['./register.css'],
})
export class Register {
  private authService = inject(AuthService);
  private router = inject(Router);
  private toast = inject(ToastService);

  protected creds: RegisterCreds = {
    firstName: '',
    lastName: '',
    username: '',
    email: '',
    password: '',
  };

  protected confirmPassword = '';

  cancelRegister = output<boolean>();
  isSubmitting = false;

  get passwordsMismatch(): boolean {
    return !!this.creds.password && !!this.confirmPassword
      ? this.creds.password !== this.confirmPassword
      : false;
  }

  register(form: NgForm) {
    if (form.invalid) {
      form.form.markAllAsTouched();
      this.toast.error('Uzupełnij poprawnie wymagane pola.');
      return;
    }
    if (this.passwordsMismatch) {
      this.toast.error('Hasła muszą być identyczne.');
      return;
    }

    this.isSubmitting = true;
    this.authService.register(this.creds).subscribe({
      next: () => {
        this.toast.success('Zarejestrowano pomyślnie');
        this.router.navigateByUrl('/');
      },
      error: (error) => {
        const msg = error?.error?.message || error?.message || 'Błąd podczas rejestracji';
        this.toast.error(msg);
        this.isSubmitting = false;
      },
      complete: () => (this.isSubmitting = false),
    });
  }

  cancel() {
    this.cancelRegister.emit(false);
    this.router.navigateByUrl('/');
  }
}
