import { Component, inject, Input, signal } from '@angular/core';
import { Register } from "../account/register/register";
import { AuthService } from '../../core/services/auth-service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-home',
  imports: [Register],
  templateUrl: './home.html',
  styleUrl: './home.css'
})
export class Home {
private router = inject(Router);
protected registerMode = signal(false);
protected authService = inject(AuthService);

showRegister(value: boolean) {
  this.registerMode.set(value);
}

showTask() {
    this.router.navigate(['/tasks']);
  }
}
