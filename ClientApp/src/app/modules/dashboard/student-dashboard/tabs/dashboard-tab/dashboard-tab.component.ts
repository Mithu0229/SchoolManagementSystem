import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';

interface StudentProfileField {
  label: string;
  value: string;
}

interface FeesDueRow {
  installment: string;
  date: string;
  amount: string;
}

interface PaidFeeRow {
  date: string;
  amount: string;
  slip: string;
}

@Component({
  selector: 'app-student-dashboard-tab-dashboard',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './dashboard-tab.component.html',
  styleUrl: './dashboard-tab.component.scss',
})
export class DashboardTabComponent {
  avatarMissing = false;

  readonly student = {
    name: 'Ibtida Elaf Jinan',
    initials: 'IJ',
  };

  readonly profileFields: StudentProfileField[] = [
    { label: 'DOB', value: '11/Feb/2015' },
    { label: 'Student ID', value: '202500580' },
    { label: 'Class', value: 'Class-5' },
    { label: 'Section', value: 'B' },
    { label: 'Shift', value: 'Morning' },
    { label: 'Version/Medium', value: 'English' },
    { label: 'Blood Group', value: 'O+' },
    { label: 'Resident', value: 'No' },
    { label: 'Email', value: 'shahincc1@gmail.com' },
    { label: 'Mobile', value: '01317770224' },
    { label: 'Telephone', value: '-' },
  ];

  readonly feeRows: FeesDueRow[] = [
    { installment: 'March', date: '-', amount: '11,190.00/-' },
  ];

  readonly paidFees: PaidFeeRow[] = [
    { date: '28/Jan/26', amount: '11090', slip: 'Print' },
    { date: '03/Jan/26', amount: '11190', slip: 'Print' },
    { date: '09/Nov/25', amount: '11090', slip: 'Print' },
  ];

  readonly classTeacher = {
    name: 'Ms. Rokhsana Titlee',
    phone: '01915686300',
  };
}
