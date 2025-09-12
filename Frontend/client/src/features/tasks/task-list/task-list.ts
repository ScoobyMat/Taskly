import { Component, inject } from '@angular/core';
import { AsyncPipe, CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { Observable } from 'rxjs';

import { TaskService } from '../../../core/services/task-service';
import { Task, TaskFilter } from '../../../shared/types/task';
import { TaskPriorityLabels } from '../../../shared/enums/task-priority.enum';
import { TaskStatusLabels } from '../../../shared/enums/task-status.enum';
import { TaskFilterComponent } from '../task-filter/task-filter';

@Component({
  selector: 'app-task-list',
  standalone: true,
  imports: [AsyncPipe, CommonModule, RouterModule, TaskFilterComponent],
  templateUrl: './task-list.html',
  styleUrls: ['./task-list.css'],
})
export class TaskList {
  private taskService = inject(TaskService);
  task$: Observable<Task[]>;

  public readonly TaskPriorityLabels = TaskPriorityLabels;
  public readonly TaskStatusLabels = TaskStatusLabels;

  constructor() {
    this.task$ = this.taskService.getTasks();
  }

  onFilterChanged(filter: TaskFilter) {
    this.task$ = this.taskService.filterTasks(filter);
  }
}
