import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject, signal } from '@angular/core';
import { environment } from '../../environments/environment';
import {
  Task,
  EditableTask,
  TaskForm,
  TaskFilter,
} from '../../shared/types/task';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class TaskService {
  private http = inject(HttpClient);
  private baseUrl = environment.apiUrl;
  editMode = signal<boolean>(false);

  getTasks(): Observable<Task[]> {
    return this.http.get<Task[]>(`${this.baseUrl}Tasks`);
  }

  getTask(id: string): Observable<Task> {
    return this.http.get<Task>(`${this.baseUrl}Tasks/${id}`);
  }

  createTask(task: TaskForm): Observable<Task> {
    return this.http.post<Task>(`${this.baseUrl}Tasks`, task);
  }

  updateTask(task: EditableTask): Observable<Task> {
    return this.http.put<Task>(`${this.baseUrl}Tasks`, task);
  }

  deleteTask(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}Tasks/${id}`);
  }

  filterTasks(filter: TaskFilter): Observable<Task[]> {
    let params = new HttpParams();

    if (filter.title) params = params.set('Title', filter.title);
    if (filter.status) params = params.set('Status', filter.status);
    if (filter.priority) params = params.set('Priority', filter.priority);
    if (filter.category) params = params.set('Category', filter.category);
    if (filter.sortByDueDate)
      params = params.set('SortByDueDate', filter.sortByDueDate);

    return this.http.post<Task[]>(`${this.baseUrl}Tasks/filter`, null, {
      params,
    });
  }
}
