import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { InstituteComponent } from './components/institute/institute.component';

import { BranchComponent } from './components/branch/branch.component';
import { FinancialYearComponent } from './components/financial-year/financial-year.component';
import { AcademicSessionComponent } from './components/academic-session/academic-session.component';
import { AcademicClassComponent } from './components/academic-class/academic-class.component';
import { SectionComponent } from './components/section/section.component';
import { ShiftComponent } from './components/shift/shift.component';
import { StudentGroupComponent } from './components/student-group/student-group.component';
import { FeeHeadComponent } from './components/fee-head/fee-head.component';
import { StudentFeeLedgerComponent } from './components/student-fee-ledger/student-fee-ledger.component';
import { StudentComponent } from './components/student/student.component';
import { StudentListComponent } from './components/student-list/student-list.component';
import { AdmissionComponent } from './components/admission/admission.component';
import { FeeTemplateComponent } from './components/fee-template/fee-template.component';
import { FeeCollectionComponent } from './components/fee-collection/fee-collection.component';
import { BillProcessComponent } from './components/bill-process/bill-process.component';
import { BillCollectionComponent } from './components/bill-collection/bill-collection.component';

import { AppLayout } from '../../shared/layout/dashboard-layout/component/app.layout';
import { authGuard } from '../../core/guards/auth.guard';
import { permissionGuard } from '../../core/guards/permission.guard';

const routes: Routes = [
  {
    path: '',
    canActivate: [authGuard],
    component: AppLayout,
    children: [
      {
        path: 'institute',
        component: InstituteComponent,
        canActivate: [authGuard, permissionGuard('/institute')],
      },
      {
        path: 'branch',
        component: BranchComponent,
        canActivate: [authGuard, permissionGuard('/branch')],
      },
      {
        path: 'financial-year',
        component: FinancialYearComponent,
        canActivate: [authGuard, permissionGuard('/financial-year')],
      },
      {
        path: 'academic-session',
        component: AcademicSessionComponent,
        canActivate: [authGuard, permissionGuard('/academic-session')],
      },
      {
        path: 'academic-class',
        component: AcademicClassComponent,
        canActivate: [authGuard, permissionGuard('/academic-class')],
      },
      {
        path: 'section',
        component: SectionComponent,
        canActivate: [authGuard, permissionGuard('/section')],
      },
      {
        path: 'shift',
        component: ShiftComponent,
        canActivate: [authGuard, permissionGuard('/shift')],
      },
      {
        path: 'student-group',
        component: StudentGroupComponent,
        canActivate: [authGuard, permissionGuard('/student-group')],
      },
      {
        path: 'fee-head',
        component: FeeHeadComponent,
        canActivate: [authGuard, permissionGuard('/fee-head')],
      },
      {
        path: 'student-fee-ledger',
        component: StudentFeeLedgerComponent,
        canActivate: [authGuard, permissionGuard('/student-fee-ledger')],
      },
      {
        path: 'student',
        component: StudentComponent,
        canActivate: [authGuard, permissionGuard('/student')],
      },
      {
        path: 'student-list',
        component: StudentListComponent,
        canActivate: [authGuard, permissionGuard('/student-list')],
      },
      {
        path: 'admission',
        component: AdmissionComponent,
        canActivate: [authGuard, permissionGuard('/admission')],
      },
      {
        path: 'fee-template',
        component: FeeTemplateComponent,
        canActivate: [authGuard, permissionGuard('/fee-template')],
      },
      {
        path: 'fee-collection',
        component: FeeCollectionComponent,
        canActivate: [authGuard, permissionGuard('/fee-collection')],
      },
      {
        path: 'bill-process',
        component: BillProcessComponent,
        canActivate: [authGuard, permissionGuard('/bill-process')],
      },
      {
        path: 'bill-collection',
        component: BillCollectionComponent,
        canActivate: [authGuard, permissionGuard('/bill-collection')],
      },
      { path: '', redirectTo: 'institute', pathMatch: 'full' },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class SchoolRoutingModule { }
