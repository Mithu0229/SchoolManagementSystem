import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { authGuard } from '../../core/guards/auth.guard';
import { permissionGuard } from '../../core/guards/permission.guard';
import { AppLayout } from '../../shared/layout/dashboard-layout/component/app.layout';

const routes: Routes = [
  {
    path: '',
    //component: AppLayout,
    //canActivate: [authGuard],
    children: [
      {
        path: '',
        loadComponent: () =>
          import(
            '../student-application/student-application-list/student-application-list.component'
          ).then((m) => m.StudentApplicationListComponent),
        //canActivate: [authGuard, permissionGuard('/student')],
        data: {
          breadcrumb: 'Student Management',
          //parent: [{ label: 'student', url: '/student' }],
        },
      },
      {
        path: 'create',
        loadComponent: () =>
          import(
            '../student-application/create-student-application/student-application.component'
          ).then((m) => m.StudentApplicationComponent),
        //canActivate: [authGuard, permissionGuard('/student')],
        data: {
          breadcrumb: 'Create Student Application',
          parent: [{ label: 'Student Application', url: '/student' }],
        },
      },
      {
        path: 'edit/:id',
        loadComponent: () =>
          import(
            '../student-application/edit-student-application/edit-student-application.component'
          ).then((m) => m.EditStudentApplicationComponent),
        // canActivate: [authGuard, permissionGuard('/student')],
        data: {
          breadcrumb: 'Edit student',
          parent: [
            { label: 'student', url: '/student' },
            { label: 'student Management', url: '/student' },
          ],
        },
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class StudentApplicationRoutingModule {}
