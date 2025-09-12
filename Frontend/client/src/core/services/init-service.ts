import { inject, Injectable } from '@angular/core';
import { AuthService } from './auth-service';
import { CategoryService } from './category-service';
import { forkJoin, of, Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class InitService {
  private authService = inject(AuthService);
  private categoryService = inject(CategoryService);

  init(): Observable<unknown> {
    const tasks: Observable<unknown>[] = [];

    const userString = localStorage.getItem('user');
    if (userString) {
      const user = JSON.parse(userString);
      this.authService.currentUser.set(user);
    }

    tasks.push(this.categoryService.loadOnce());

    return tasks.length ? forkJoin(tasks) : of(void 0);
  }
}
