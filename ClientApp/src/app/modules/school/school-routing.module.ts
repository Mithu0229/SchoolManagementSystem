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

const routes: Routes = [
  { path: 'institute', component: InstituteComponent },
  { path: 'branch', component: BranchComponent },
  { path: 'financial-year', component: FinancialYearComponent },
  { path: 'academic-session', component: AcademicSessionComponent },
  { path: 'academic-class', component: AcademicClassComponent },
  { path: 'section', component: SectionComponent },
  { path: 'shift', component: ShiftComponent },
  { path: 'student-group', component: StudentGroupComponent },
  { path: 'fee-head', component: FeeHeadComponent },
  { path: 'student-fee-ledger', component: StudentFeeLedgerComponent },
  { path: '', redirectTo: 'institute', pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SchoolRoutingModule { }
