export enum TaskPriority {
  Low = 'Low',
  Medium = 'Medium',
  High = 'High',
}

export const TaskPriorityLabels: Record<TaskPriority, string> = {
  [TaskPriority.Low]: 'Niski',
  [TaskPriority.Medium]: 'Średni',
  [TaskPriority.High]: 'Wysoki',
};
