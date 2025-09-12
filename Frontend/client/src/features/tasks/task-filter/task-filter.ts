import { Component, EventEmitter, Output, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { TaskPriority, TaskPriorityLabels } from '../../../shared/enums/task-priority.enum';
import { TaskStatus, TaskStatusLabels } from '../../../shared/enums/task-status.enum';
import { TaskFilter as ITaskFilter } from '../../../shared/types/task';
import { CategoryService } from '../../../core/services/category-service';

@Component({
  selector: 'app-task-filter',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './task-filter.html',
})
export class TaskFilterComponent implements OnInit {
  @Output() filterChanged = new EventEmitter<ITaskFilter>();

  private categoryService = inject(CategoryService);

  title = '';
  status: TaskStatus | undefined = undefined;
  priority: TaskPriority | undefined = undefined;
  category: string | undefined = undefined;
  sortByDueDate: 'asc' | 'desc' | undefined = undefined;

  taskPriorities = Object.values(TaskPriority) as TaskPriority[];
  taskStatuses = Object.values(TaskStatus) as TaskStatus[];
  TaskPriorityLabels = TaskPriorityLabels;
  TaskStatusLabels = TaskStatusLabels;

  categories = this.categoryService.categories;

  ngOnInit(): void {
    this.categoryService.loadOnce().subscribe();
  }

  onFilter(): void {
    const filter: ITaskFilter = {};

    if (this.title.trim()) filter.title = this.title.trim();
    if (this.status) filter.status = this.status;
    if (this.priority) filter.priority = this.priority;
    if (this.category) filter.category = this.category;
    if (this.sortByDueDate) filter.sortByDueDate = this.sortByDueDate;

    this.filterChanged.emit(filter);
  }

  onReset(): void {
    this.title = '';
    this.status = undefined;
    this.priority = undefined;
    this.category = undefined;
    this.sortByDueDate = undefined;
    this.onFilter();
  }
}
