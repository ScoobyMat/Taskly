import { Component, inject, OnInit, signal, ChangeDetectorRef} from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { TaskService } from '../../../core/services/task-service';
import { ActivatedRoute, Router } from '@angular/router';
import { EditableTask, TaskPriority, TaskStatus, } from '../../../shared/types/task';
import { CategoryService } from '../../../core/services/category-service';
import { firstValueFrom } from 'rxjs';
import { TaskPriorityLabels } from '../../../shared/enums/task-priority.enum';
import { TaskStatusLabels } from '../../../shared/enums/task-status.enum';
import { ToastService } from '../../../core/services/toast-service';

@Component({
  selector: 'app-task-edit',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './task-edit.html',
  styleUrls: ['./task-edit.css'],
})
export class TaskEdit implements OnInit {
  private taskService = inject(TaskService);
  private categoryService = inject(CategoryService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private cdr = inject(ChangeDetectorRef);
  private toast = inject(ToastService);

  categories = this.categoryService.categories;

  loading = signal(true);
  error = signal<string | null>(null);
  isSubmitting = false;

  editTask: EditableTask | null = null;

  TaskPriorityLabels = TaskPriorityLabels;
  TaskStatusLabels = TaskStatusLabels;

  taskStatuses = Object.values(TaskStatus);
  taskPriorities = Object.values(TaskPriority);

  public today: string = (() => {
    const d = new Date();
    return `${d.getFullYear()}-${String(d.getMonth() + 1).padStart(
      2,
      '0'
    )}-${String(d.getDate()).padStart(2, '0')}`;
  })();

  async ngOnInit() {
    try {
      const id = this.route.snapshot.paramMap.get('id')!;
      const task = await firstValueFrom(this.taskService.getTask(id));
      this.editTask = {
        id: task.id,
        title: task.title,
        description: task.description,
        status: task.status,
        priority: task.priority ?? null,
        dueDate: task.dueDate,
        category: task.category,
      };
    } catch (err) {
      console.error('[TaskEdit] getTask error', err);
      this.toast.error('Nie udało się pobrać zadania.');
    } finally {
      this.loading.set(false);
      this.cdr.markForCheck();
    }
  }

  updateTask(formRef: NgForm) {
    if (!this.editTask) return;

    if (formRef.invalid) {
      formRef.form.markAllAsTouched();
      this.toast.error('Uzupełnij wymagane pola.');
      return;
    }

    this.isSubmitting = true;

    this.taskService.updateTask(this.editTask).subscribe({
      next: () => {
        this.toast.success('Zadanie zaktualizowano pomyślnie');
        this.router.navigate(['/tasks', this.editTask!.id]);
      },
      error: (err) => {
        console.error('[TaskEdit] update error', err);
        this.toast.error('Nie udało się zapisać zmian.');
      },
      complete: () => (this.isSubmitting = false),
    });
  }

  cancel() {
    if (this.editTask) {
      this.router.navigateByUrl('/tasks/' + this.editTask.id);
    } else {
      this.router.navigateByUrl('/tasks');
    }
  }
}
