export enum TaskStatus {
  New = 'New',
  InProgress = 'InProgress',
  Done = 'Done',
  Cancelled = 'Cancelled',
}

export const TaskStatusLabels: Record<TaskStatus, string> = {
  [TaskStatus.New]: 'Nowy',
  [TaskStatus.InProgress]: 'W trakcie realizacji',
  [TaskStatus.Done]: 'Zakończony',
  [TaskStatus.Cancelled]: 'Anulowany',
};
