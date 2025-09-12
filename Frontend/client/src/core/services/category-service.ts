import { Injectable, inject, signal, computed } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Category } from '../../shared/types/task';
import { environment } from '../../environments/environment';
import { of, tap, catchError } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class CategoryService {
  private http = inject(HttpClient);
  private baseUrl = environment.apiUrl;

  private _categories = signal<Category[]>([]);
  private _loaded = signal(false);

  readonly categories = computed(() => this._categories());

  loadOnce() {
    if (this._loaded()) {
      return of(this._categories());
    }
    return this.http.get<Category[]>(`${this.baseUrl}Category`).pipe(
      tap((list) => {
        this._categories.set(list ?? []);
        this._loaded.set(true);
      }),
      catchError((err) => {
        console.error('Category load error', err);
        this._loaded.set(false);
        this._categories.set([]);
        return of([]);
      })
    );
  }
}
