import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AppLayout } from '../../../shared/layout/dashboard-layout/component/app.layout';
import { UserLoginComponent } from '../../auth/user-login/user-login.component';
import { authGuard } from '../../../core/guards/auth.guard';
import { UserListComponent } from './user-list/user-list.component';

const routes: Routes = [
  {
    path: '',
    component: AppLayout,
    //canActivate: [authGuard],
    children: [
      {
        path: '',
        loadComponent: () =>
          import(
            '../../dashboard/admin-dashboard/admin-dashboard.component'
          ).then((m) => m.AdminDashboardComponent),
        //canActivate: [authGuard],
      },
      {
        path: 'member-list',
        component: UserListComponent,
        // loadComponent: () =>
        //   import('./user-list/user-list.component').then(
        //     (m) => m.UserListComponent
        //   ),
        //canActivate: [authGuard],
      },
      {
        path: 'create',
        loadComponent: () =>
          import('./create-user/create-user.component').then(
            (m) => m.CreateUserComponent
          ),
      },
      {
        path: 'edit/:id',
        loadComponent: () =>
          import('./edit-user/edit-user.component').then(
            (m) => m.EditUserComponent
          ),
        //canActivate: [authGuard],
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class UserRoutingModule {}
