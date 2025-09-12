import { TaskPriority } from "../enums/task-priority.enum";
import { TaskStatus } from "../enums/task-status.enum";

export type Task = {
  id: string;
  title: string;
  description: string;
  status: TaskStatus;
  priority: TaskPriority;
  createdAt: string;
  updatedAt: string | null;
  dueDate: string;
  category: string;
  userId: string;
};

export type EditableTask = {
  id: string;
  title: string;
  description: string;
  status: TaskStatus;
  priority: TaskPriority | null;
  dueDate: string;
  category: string;
};

export type TaskForm = {
  title: string;
  description: string;
  priority?: TaskPriority | null;
  dueDate: string;
  category: string;
};


export type Category = {
  id: string;
  name: string;
};

export type TaskFilter = {
  title?: string;
  status?: TaskStatus;
  priority?: TaskPriority;
  category?: string;
  sortByDueDate?: 'asc' | 'desc';
};

export { TaskPriority, TaskStatus };

