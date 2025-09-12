import { Component, inject } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { TaskForm, TaskPriority } from '../../../shared/types/task';
import { TaskService } from '../../../core/services/task-service';
import { ToastService } from '../../../core/services/toast-service';
import { CategoryService } from '../../../core/services/category-service';
import { TaskPriorityLabels } from '../../../shared/enums/task-priority.enum';

@Component({
  selector: 'app-task-create',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './task-create.html',
  styleUrls: ['./task-create.css'],
})
export class TaskCreate {
  private taskService = inject(TaskService);
  private categoryService = inject(CategoryService);
  private toast = inject(ToastService);
  private router = inject(Router);

  categories = this.categoryService.categories;
  TaskPriorityLabels = TaskPriorityLabels;
  taskPriorities = Object.values(TaskPriority);

  isSubmitting = false;

  public today: string = (() => {
    const d = new Date();
    return `${d.getFullYear()}-${String(d.getMonth() + 1).padStart(
      2,
      '0'
    )}-${String(d.getDate()).padStart(2, '0')}`;
  })();

  newTask: TaskForm = {
    title: '',
    description: '',
    priority: null,
    dueDate: '',
    category: '',
  };

  createTask(form: NgForm): void {
    if (form.invalid) {
      form.form.markAllAsTouched();
      this.toast.error('Uzupełnij wymagane pola.');
      return;
    }

    this.isSubmitting = true;

    const taskToCreate: TaskForm = {
      ...this.newTask,
      dueDate: this.formatDueDate(this.newTask.dueDate),
    };

    this.taskService.createTask(taskToCreate).subscribe({
      next: () => {
        this.toast.success('Zadanie utworzono pomyślnie');
        this.router.navigateByUrl('/tasks');
      },
      error: (err) => {
        console.error('Create error', err);
        this.toast.error('Błąd podczas tworzenia zadania');
      },
      complete: () => (this.isSubmitting = false),
    });
  }

  private formatDueDate(d?: string): string {
    return d ? new Date(d + 'T00:00:00').toISOString() : '';
  }

  cancel() {
    this.router.navigateByUrl('/tasks');
  }
}
