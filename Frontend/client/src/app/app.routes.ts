import { Routes } from '@angular/router';
import { Home } from '../features/home/home';
import { Login } from '../features/account/login/login';
import { Register } from '../features/account/register/register';
import { TaskList } from '../features/tasks/task-list/task-list';
import { TaskDetailed } from '../features/tasks/task-detailed/task-detailed';
import { TaskEdit } from '../features/tasks/task-edit/task-edit';
import { authGuard } from '../core/guards/auth-guard';
import { TaskCreate } from '../features/tasks/task-create/task-create';

export const routes: Routes = [
  { path: '', component: Home },
  { path: 'login', component: Login },
  { path: 'register', component: Register },
  {
    path: '',
    runGuardsAndResolvers: 'always',
    canActivate: [authGuard],
    children: [
      { path: 'tasks', component: TaskList },
      { path: 'tasks/create', component: TaskCreate },
      { path: 'tasks/:id', component: TaskDetailed },
      { path: 'tasks/:id/edit', component: TaskEdit },
    ],
  },
  { path: '**', component: Home },
];
