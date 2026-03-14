import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  ReactiveFormsModule,
  FormBuilder,
  FormGroup,
  Validators,
  FormControl,
} from '@angular/forms';
import { StudentService } from '../../../core/services/student.service';
import { ToastService } from '../../../core/services/toast.service';
import { FormBase } from '../../../core/enums/form-base';
import { ButtonModule } from 'primeng/button';
import { FormErrorComponent } from '../../../shared/components/form-error.component';
import { InputTextModule } from 'primeng/inputtext';
import { SelectModule } from 'primeng/select';
import { CheckboxModule } from 'primeng/checkbox';
import { DatePickerModule } from 'primeng/datepicker';
import { TextareaModule } from 'primeng/textarea';
import { Router } from '@angular/router';

@Component({
  selector: 'app-student-application',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    CheckboxModule,
    CommonModule,
    ButtonModule,
    ReactiveFormsModule,
    FormErrorComponent,
    InputTextModule,
    SelectModule,
    DatePickerModule,
    TextareaModule,
  ],
  templateUrl: './student-application.component.html',
  styles: [
    `
      @keyframes fadeInUp {
        from {
          opacity: 0;
          transform: translateY(20px);
        }
        to {
          opacity: 1;
          transform: translateY(0);
        }
      }
      .animate-fade-in-up {
        animation: fadeInUp 0.5s ease-out forwards;
      }

      .row {
        display: flex;
        align-items: center;
        gap: 20px;
      }

      .image-preview img {
        width: 120px;
        height: 120px;
        object-fit: cover;
        border-radius: 6px;
        border: 1px solid #ddd;
      }
    `,
  ],
})
export class StudentApplicationComponent extends FormBase implements OnInit {
  activeTab: 'student' | 'guardian' | 'localGuardian' = 'student';
  applicationForm!: FormGroup;
  genderList = [
    { label: 'Male', value: '1' },
    { label: 'Female', value: '2' },
    { label: 'Other', value: '3' },
  ];
  religionList = [
    { label: 'Christianity', value: 'Christianity' },
    { label: 'Islam', value: 'Islam' },
    { label: 'Hinduism', value: 'Hinduism' },
    { label: 'Buddhism', value: 'Buddhism' },
    { label: 'Sikhism', value: 'Sikhism' },
    { label: 'Judaism', value: 'Judaism' },
    { label: 'Other', value: 'Other' },
  ];
  bloodGroupList = [
    { label: 'A+', value: 'A+' },
    { label: 'A-', value: 'A-' },
    { label: 'B+', value: 'B+' },
    { label: 'B-', value: 'B-' },
    { label: 'AB+', value: 'AB+' },
    { label: 'AB-', value: 'AB-' },
    { label: 'O+', value: 'O+' },
    { label: 'O-', value: 'O-' },
  ];
  applicationForClassList = [
    { label: 'Nursery', value: 'Nursery' },
    { label: 'Kindergarten', value: 'Kindergarten' },
    { label: '1st Grade', value: '1st Grade' },
    { label: '2nd Grade', value: '2nd Grade' },
    { label: '3rd Grade', value: '3rd Grade' },
    { label: '4th Grade', value: '4th Grade' },
    { label: '5th Grade', value: '5th Grade' },
    { label: '6th Grade', value: '6th Grade' },
  ];
  isDisabilityList = [
    { label: 'Yes', value: true },
    { label: 'No', value: false },
  ];
  selectedFile!: File | null;
  imagePreview: string | ArrayBuffer | null = null;

  constructor(
    private fb: FormBuilder,
    private studentService: StudentService,
    private toastService: ToastService,
    private readonly router: Router,
  ) {
    super();
    this.buildForm();
  }

  ngOnInit() {}
  get form(): FormGroup {
    return this.applicationForm;
  }

  onFileChange(event: Event) {
    const input = event.target as HTMLInputElement;

    if (!input.files || input.files.length === 0) return;

    this.selectedFile = input.files[0];

    const reader = new FileReader();
    reader.onload = () => {
      this.imagePreview = reader.result;
    };
    reader.readAsDataURL(this.selectedFile);
  }

  buildForm() {
    this.applicationForm = this.fb.group({
      student: this.fb.group({
        //id: new FormControl(''),
        fullName: new FormControl('', [Validators.required]),
        gender: new FormControl(null, [Validators.required]),
        dateOfBirth: new FormControl(null, [Validators.required]),
        placeOfBirth: new FormControl('', [Validators.required]),
        nationality: new FormControl('', [Validators.required]),
        religion: new FormControl('', [Validators.required]),
        bloodGroup: new FormControl('', [Validators.required]),
        birthCertificateNo: new FormControl('', [Validators.required]),
        applicationForClass: new FormControl('', [Validators.required]),
        academicYear: new FormControl('', [Validators.required]),
        lastSchool: new FormControl(''),
        lastClassAttendedResult: new FormControl(''),
        isDisability: new FormControl(false),
        disability: new FormControl(''),
        specialCare: new FormControl(''),
        presentAddress: new FormControl('', [Validators.required]),
        permanentAddress: new FormControl('', [Validators.required]),
        studentPhone: new FormControl(''),
        studentEmail: new FormControl(''),
        image: new FormControl(null, [Validators.required]),
      }),
      guardian: this.fb.group({
        //id: new FormControl(''),
        fatherName: new FormControl('', [Validators.required]),
        fatherAcademicQualification: new FormControl(''),
        fatherOccupation: new FormControl(''),
        fatherMobile: new FormControl('', [Validators.required]),
        fatherTelephoneOffice: new FormControl(''),
        fatherTelephoneResidence: new FormControl(''),
        fatherEmail: new FormControl(''),
        motherName: new FormControl('', [Validators.required]),
        motherAcademicQualification: new FormControl(''),
        motherOccupation: new FormControl(''),
        motherMobile: new FormControl('', [Validators.required]),
        motherTelephoneOffice: new FormControl(''),
        motherTelephoneResidence: new FormControl(''),
        motherEmail: new FormControl(''),
      }),
      localGuardian: this.fb.group({
        //id: new FormControl(''),
        name: new FormControl(''),
        relationToStudent: new FormControl(''),
        address: new FormControl(''),
        phone: new FormControl(''),
        email: new FormControl(''),
      }),
    });
  }

  setActiveTab(tab: 'student' | 'guardian' | 'localGuardian') {
    this.activeTab = tab;
  }

  onSubmit() {
    this.markAllAsTouched();
    if (this.applicationForm.valid) {
      const formData = new FormData();

      const student = this.applicationForm.value.student;
      const guardian = this.applicationForm.value.guardian;
      const localGuardian = this.applicationForm.value.localGuardian;

      // ✅ Student fields
      formData.append('FullName', student.fullName);
      formData.append('Gender', student.gender);
      formData.append('DateOfBirth', student.dateOfBirth);
      formData.append('PlaceOfBirth', student.placeOfBirth);
      formData.append('Nationality', student.nationality);
      formData.append('Religion', student.religion);
      formData.append('BloodGroup', student.bloodGroup);
      formData.append('BirthCertificateNo', student.birthCertificateNo);
      formData.append('ApplicationForClass', student.applicationForClass);
      formData.append('AcademicYear', student.academicYear);
      formData.append('PresentAddress', student.presentAddress);
      formData.append('PermanentAddress', student.permanentAddress);

      // ✅ Optional
      formData.append('LastSchool', student.lastSchool ?? '');
      formData.append('StudentPhone', student.studentPhone ?? '');
      formData.append('StudentEmail', student.studentEmail ?? '');
      formData.append('IsDisability', student.isDisability ?? false);
      formData.append('Disability', student.disability ?? '');
      formData.append('SpecialCare', student.specialCare ?? '');
      formData.append(
        'LastClassAttendedResult',
        student.lastClassAttendedResult ?? '',
      );

      // ✅ Guardian - Father (nested binding for [FromForm] StudentInfoRequest)
      formData.append('GuardianInfo.FatherName', guardian.fatherName ?? '');
      formData.append(
        'GuardianInfo.FatherOccupation',
        guardian.fatherOccupation ?? '',
      );
      formData.append(
        'GuardianInfo.FatherAcademicQualification',
        guardian.fatherAcademicQualification ?? '',
      );
      formData.append('GuardianInfo.FatherMobile', guardian.fatherMobile ?? '');
      formData.append('GuardianInfo.FatherEmail', guardian.fatherEmail ?? '');
      formData.append(
        'GuardianInfo.FatherTelephoneOffice',
        guardian.fatherTelephoneOffice ?? '',
      );
      formData.append(
        'GuardianInfo.FatherTelephoneResidence',
        guardian.fatherTelephoneResidence ?? '',
      );
      // formData.append(
      //   'GuardianInfo.FatherNationalIdNo',
      //   guardian.fatherNationalIdNo ?? '',
      // );

      // ✅ Guardian - Mother
      formData.append('GuardianInfo.MotherName', guardian.motherName ?? '');
      formData.append(
        'GuardianInfo.MotherOccupation',
        guardian.motherOccupation ?? '',
      );
      formData.append(
        'GuardianInfo.MotherAcademicQualification',
        guardian.motherAcademicQualification ?? '',
      );
      formData.append('GuardianInfo.MotherMobile', guardian.motherMobile ?? '');
      formData.append('GuardianInfo.MotherEmail', guardian.motherEmail ?? '');
      formData.append(
        'GuardianInfo.MotherTelephoneOffice',
        guardian.motherTelephoneOffice ?? '',
      );
      formData.append(
        'GuardianInfo.MotherTelephoneResidence',
        guardian.motherTelephoneResidence ?? '',
      );
      // formData.append(
      //   'GuardianInfo.MotherNationalIdNo',
      //   guardian.motherNationalIdNo ?? '',
      // );

      // ✅ Local Guardian (nested binding for [FromForm] StudentInfoRequest)
      formData.append('LocalGuardianInfo.Name', localGuardian.name ?? '');
      formData.append(
        'LocalGuardianInfo.RelationToStudent',
        localGuardian.relationToStudent ?? '',
      );
      formData.append('LocalGuardianInfo.Phone', localGuardian.phone ?? '');
      formData.append('LocalGuardianInfo.Email', localGuardian.email ?? '');
      formData.append('LocalGuardianInfo.Address', localGuardian.address ?? '');

      // ✅ Image
      if (this.selectedFile) {
        formData.append('Image', this.selectedFile);
      }

      this.studentService.createStudent(formData).subscribe({
        next: (res) => {
          this.router.navigateByUrl('/application');
          if (res.isSuccess) {
            this.toastService.success('Sitemap has been created successfully.');
            // this.resetForm();
          } else {
            let errorMessage =
              'Failed to create sitemap. Please try again later.';
            if (res.notificationMessage && res.notificationMessage !== '') {
              errorMessage = res.notificationMessage;
            } else if (res.errors?.[0]) {
              errorMessage = res.errors[0];
            }
            this.toastService.error(errorMessage);
          }
        },
        error: () => {
          this.toastService.error(
            'Failed to create sitemap. Please try again later.',
          );
        },
      });
      console.log('Full Form Value:', this.applicationForm.value);
    } else {
      console.log('Form Invalid');
      this.applicationForm.markAllAsTouched();
    }
  }

  get studentGroup() {
    return this.applicationForm.get('student') as FormGroup;
  }
}
