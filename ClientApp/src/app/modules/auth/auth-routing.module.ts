import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AppLayout } from '../../shared/layout/dashboard-layout/component/app.layout';
import { Dashboard } from '../dashboard/dashboard';
import { authGuard } from '../../core/guards/auth.guard';
import { HomeLayout } from '../../shared/layout/home-layout/component/home.layout';
import { UserRegisterComponent } from './user-register/user-register.component';
import { publicGuard } from '../../core/guards/public.guard';
import { MemberDashboardComponent } from '../dashboard/member-dashboard/member-dashboard.component';
import { UserLoginComponent } from './user-login/user-login.component';

const routes: Routes = [
  {
    path: '',
    component: AppLayout,
    children: [{ path: '', component: Dashboard, canActivate: [authGuard] }],
    // children: [
    //   {
    //     path: '',
    //     component: MemberDashboardComponent,
    //     //canActivate: [authGuard],
    //   },
    // ],
  },
  {
    path: 'login',
    component: UserLoginComponent,
    canActivate: [publicGuard],
  },
  {
    path: 'member',
    component: MemberDashboardComponent,
    // canActivate: [authGuard],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AuthRoutingModule {}
