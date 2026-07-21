import { CommonModule } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { StudentService } from '../../../../../core/services/student.service';
import { DialogModule } from 'primeng/dialog';
import { ButtonModule } from 'primeng/button';
import { MessageService } from 'primeng/api';
import { BillMasterService } from '../../../../school/services/bill-master.service';

interface StudentProfileField {
  label: string;
  value: string;
  key?: string;
}

interface FeesDueRow {
  installment: string;
  date: string;
  amount: string;
}

interface PaidFeeRow {
  id: string;
  date: string;
  amount: string;
  slip: string;
}

@Component({
  selector: 'app-student-dashboard-tab-dashboard',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, DialogModule, ButtonModule],
  providers: [MessageService],
  templateUrl: './dashboard-tab.component.html',
  styleUrl: './dashboard-tab.component.scss',
})
export class DashboardTabComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private studentService = inject(StudentService);
  private fb = inject(FormBuilder);
  private billMasterService = inject(BillMasterService);
  private messageService = inject(MessageService);

  reportDialog: boolean = false;
  currentReceipt: any = null;

  avatarMissing = false;
  studentId: string | null = null;
  isEditing = false;
  studentForm!: FormGroup;
  studentData: any = {};

  student = {
    name: 'Ibtida Elaf Jinan',
    initials: 'IJ',
  };

  profileFields: StudentProfileField[] = [
    { label: 'DOB', value: '11/Feb/2015', key: 'dateOfBirth' },
    { label: 'Student ID', value: '202500580', key: 'stdCID' },
    { label: 'Class', value: 'Class-5', key: 'classFor' },
    { label: 'Section', value: 'B', key: 'section' },
    { label: 'Shift', value: 'Morning', key: 'shift' },
    // { label: 'Version/Medium', value: 'English', key: 'versionName' },
    { label: 'Blood Group', value: 'O+', key: 'bloodGroup' },
    // { label: 'Resident', value: 'No', key: 'isResident' },
    { label: 'Email', value: 'shahincc1@gmail.com', key: 'email' },
    { label: 'Mobile', value: '01317770224', key: 'mobileNo' },
    // { label: 'Telephone', value: '-', key: 'telephone' },
  ];

  feeRows: FeesDueRow[] = [];

  paidFees: PaidFeeRow[] = [];

  readonly classTeacher = {
    name: 'Ms. Rokhsana Titlee',
    phone: '01915686300',
  };

  ngOnInit(): void {
    this.initForm();
    this.route.queryParams.subscribe((params) => {
      this.studentId = params['studentId'];
      if (this.studentId) {
        this.loadStudentData();
        this.loadFeesDue();
        this.loadPaidFees();
      }
    });
  }

  initForm(): void {
    this.studentForm = this.fb.group({
      id: [''],
      studentName: [''],
      dateOfBirth: [''],
      stdCID: [''],
      classFor: [''],
      section: [''],
      shift: [''],
      //versionName: [''],
      bloodGroup: [''],
      // isResident: [''],
      email: ['', [Validators.email]],
      mobileNo: [''],
      //telephone: [''],
    });
  }

  loadStudentData(): void {
    if (!this.studentId) return;
    this.studentService.getStudentById(this.studentId).subscribe({
      next: (res) => {
        debugger;
        if (res.isSuccess && res.data) {
          this.studentData = res.data;
          this.student.name = this.studentData.fullName || this.student.name;
          this.student.initials = this.getInitials(this.student.name);

          this.studentForm.patchValue(this.studentData);
          this.updateProfileFields(this.studentData);
        }
      },
      error: (err) => console.error('Error fetching student data:', err),
    });
  }

  loadFeesDue(): void {
    if (!this.studentId) return;
    const now = new Date();
    const currentMonth = now.getMonth() + 1;
    const currentYear = now.getFullYear();

    this.studentService.getFeesDueByStudent(this.studentId, currentMonth, currentYear).subscribe({
      next: (res) => {
        if (res.isSuccess && res.data) {
          this.feeRows = res.data;
        }
      },
      error: (err) => console.error('Error fetching fees due data:', err),
    });
  }

  loadPaidFees(): void {
    if (!this.studentId) return;

    this.studentService.getPaidFeesByStudent(this.studentId, false).subscribe({
      next: (res) => {
        if (res.isSuccess && res.data) {
          this.paidFees = res.data;
        }
      },
      error: (err) => console.error('Error fetching paid fees data:', err),
    });
  }

  viewReport(row: PaidFeeRow): void {
    if (!row.id) return;
    this.billMasterService.getMoneyReceipt(row.id).subscribe({
      next: (res) => {
        if (res.isSuccess && res.data) {
          this.currentReceipt = res.data;
          this.reportDialog = true;
        } else {
          this.messageService.add({
            severity: 'error',
            summary: 'Error',
            detail: 'Failed to load receipt',
          });
        }
      },
      error: () => {
        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail: 'Failed to load receipt',
        });
      },
    });
  }

  hideReportDialog(): void {
    this.reportDialog = false;
    this.currentReceipt = null;
  }

  printReport(): void {
    const printContent = document.getElementById('print-section');
    if (printContent) {
      const originalContents = document.body.innerHTML;
      document.body.innerHTML = printContent.innerHTML;
      window.print();
      document.body.innerHTML = originalContents;
      window.location.reload(); // Reload to restore angular state after replacing body
    }
  }

  updateProfileFields(data: any): void {
    this.profileFields = this.profileFields.map((field) => {
      if (
        field.key &&
        data[field.key] !== undefined &&
        data[field.key] !== null
      ) {
        return { ...field, value: String(data[field.key]) };
      }
      return field;
    });
  }

  getInitials(name: string): string {
    return name
      .split(' ')
      .map((n) => n[0])
      .join('')
      .substring(0, 2)
      .toUpperCase();
  }

  toggleEdit(): void {
    this.isEditing = !this.isEditing;
    if (this.isEditing) {
      this.studentForm.patchValue(this.studentData);
    }
  }

  saveProfile(): void {
    debugger;
    if (this.studentForm.invalid) return;

    const payload = this.studentForm.value;
    // ensure ID is passed along for the update
    payload.id = this.studentId;

    this.studentService.updateStudentOnly(payload).subscribe({
      next: (res) => {
        if (res.isSuccess) {
          this.studentData = { ...this.studentData, ...payload };
          this.student.name = this.studentData.studentName || this.student.name;
          this.student.initials = this.getInitials(this.student.name);
          this.updateProfileFields(this.studentData);
          this.isEditing = false;
        } else {
          console.error('Update failed:', res.errors);
        }
      },
      error: (err) => console.error('Error updating student data:', err),
    });
  }

  cancelEdit(): void {
    this.isEditing = false;
    this.studentForm.patchValue(this.studentData);
  }
}
