import { ToastService } from './../../../core/services/toast-service';
import { Component, inject, OnInit } from '@angular/core';
import { TaskService } from '../../../core/services/task-service';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { Observable } from 'rxjs';
import { Task } from '../../../shared/types/task';
import { AsyncPipe, DatePipe } from '@angular/common';

@Component({
  selector: 'app-task-detailed',
  standalone: true,
  imports: [AsyncPipe, DatePipe, RouterModule],
  templateUrl: './task-detailed.html',
  styleUrls: ['./task-detailed.css'],
})
export class TaskDetailed implements OnInit {
  private taskService = inject(TaskService);
  private toast = inject(ToastService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);

  task$?: Observable<Task>;
  taskId: string | null = null;

  ngOnInit(): void {
    this.taskId = this.route.snapshot.paramMap.get('id');
    if (this.taskId) {
      this.task$ = this.taskService.getTask(this.taskId);
    }
  }

  confirmDelete(): void {
    if (!this.taskId) return;

    this.taskService.deleteTask(this.taskId).subscribe({
      next: () => {
        this.toast.success('Zadanie zostało usunięte');
        this.router.navigate(['/tasks']);
      },
      error: (err) => {
        console.error('Błąd podczas usuwania zadania:', err);
        this.toast.error('Nie udało się usunąć zadania');
      },
    });

    const modalCheckbox = document.getElementById(
      'delete-modal'
    ) as HTMLInputElement;
    if (modalCheckbox) modalCheckbox.checked = false;
  }

  cancel() {
    this.router.navigateByUrl('/tasks');
  }
}
