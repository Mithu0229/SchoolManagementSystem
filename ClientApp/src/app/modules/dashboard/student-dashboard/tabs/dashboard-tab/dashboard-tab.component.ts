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
  date: string;
  amount: string;
  slip: string;
}

@Component({
  selector: 'app-student-dashboard-tab-dashboard',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './dashboard-tab.component.html',
  styleUrl: './dashboard-tab.component.scss',
})
export class DashboardTabComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private studentService = inject(StudentService);
  private fb = inject(FormBuilder);

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

  ngOnInit(): void {
    this.initForm();
    this.route.queryParams.subscribe((params) => {
      this.studentId = params['studentId'];
      if (this.studentId) {
        this.loadStudentData();
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
