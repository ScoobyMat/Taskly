import { inject, Injectable } from '@angular/core';
import { AccountService } from './account-service';
import { CategoryService } from './category-service';
import { forkJoin, of } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class InitService {
  private accountService = inject(AccountService);
  private categoryService = inject(CategoryService);

  init() {
    const tasks: any[] = [];

    const userString = localStorage.getItem('user');
    if (userString) {
      const user = JSON.parse(userString);
      this.accountService.currentUser.set(user);
    }

    tasks.push(this.categoryService.loadOnce());
    return tasks.length ? forkJoin(tasks) : of(null);
  }
}
