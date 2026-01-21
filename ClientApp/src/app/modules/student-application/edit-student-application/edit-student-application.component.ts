import { Component, OnDestroy, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { CommonModule } from '@angular/common';
import { InputTextModule } from 'primeng/inputtext';
import { TextareaModule } from 'primeng/textarea';
import { SelectModule } from 'primeng/select';
import { ButtonModule } from 'primeng/button';
import { ActivatedRoute, Router } from '@angular/router';

import { Subject, takeUntil } from 'rxjs';
import { MultiSelectModule } from 'primeng/multiselect';
import { TextareaComponent } from '../../../shared/components/textarea/textarea.component';
import { FormErrorComponent } from '../../../shared/components/form-error.component';
import { FormBase } from '../../../core/enums/form-base';
import { ToastService } from '../../../core/services/toast.service';
import { StudentService } from '../../../core/services/student.service';

@Component({
  selector: 'app-edit-student-application',
  imports: [
    ReactiveFormsModule,
    CommonModule,
    InputTextModule,
    TextareaModule,
    SelectModule,
    ButtonModule,
    TextareaComponent,
    FormErrorComponent,
    MultiSelectModule,
  ],
  templateUrl: './edit-student-application.component.html',
  styleUrl: './edit-student-application.component.scss',
})
export class EditStudentApplicationComponent
  extends FormBase
  implements OnInit
{
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
    private route: ActivatedRoute
  ) {
    super();
    this.buildForm();
  }

  ngOnInit() {
    this.route.params.subscribe((params) => {
      if (params['id']) {
        let id = params['id'];
        this.getStudentById(id);
      }
    });
  }
  get form(): FormGroup {
    return this.applicationForm;
  }
  private readonly destroy$ = new Subject<void>();

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
  getStudentById(id: string) {
    this.studentService
      .getStudentById(id)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (res) => {
          if (res.isSuccess) {
            this.patchApplicationForm(res.data);
          }
        },
      });
  }

  patchApplicationForm(data: any) {
    if (!data) return;
    debugger;
    // Student
    this.applicationForm.get('student')?.patchValue({
      fullName: data?.fullName,
      gender: data?.gender,
      dateOfBirth: data?.dateOfBirth,
      placeOfBirth: data?.placeOfBirth,
      nationality: data?.nationality,
      religion: data?.religion,
      bloodGroup: data?.bloodGroup,
      birthCertificateNo: data?.birthCertificateNo,
      applicationForClass: data?.applicationForClass,
      academicYear: data?.academicYear,
      lastSchool: data?.lastSchool,
      studentPhone: data?.studentPhone,
      studentEmail: data?.studentEmail,
      isDisability: data?.isDisability,
      disability: data?.disability,
      specialCare: data?.specialCare,
      presentAddress: data?.presentAddress,
      permanentAddress: data?.permanentAddress,
    });

    // Guardian
    this.applicationForm.get('guardian')?.patchValue({
      fatherName: data.guardianInfo?.fatherName,
      fatherOccupation: data.guardianInfo?.fatherOccupation,
      fatherAcademicQualification:
        data.guardianInfo?.fatherAcademicQualification,
      fatherMobile: data.guardianInfo?.fatherMobile,
      fatherEmail: data.guardianInfo?.fatherEmail,
      fatherTelephoneOffice: data.guardianInfo?.fatherTelephoneOffice,
      fatherTelephoneResidence: data.guardianInfo?.fatherTelephoneResidence,

      motherName: data.guardianInfo?.motherName,
      motherOccupation: data.guardianInfo?.motherOccupation,
      motherAcademicQualification:
        data.guardianInfo?.motherAcademicQualification,
      motherMobile: data.guardianInfo?.motherMobile,
      motherEmail: data.guardianInfo?.motherEmail,
      motherTelephoneOffice: data.guardianInfo?.motherTelephoneOffice,
      motherTelephoneResidence: data.guardianInfo?.motherTelephoneResidence,
    });

    // Local Guardian
    this.applicationForm.get('localGuardian')?.patchValue({
      name: data.localGuardianInfo?.name,
      relationToStudent: data.localGuardianInfo?.relationToStudent,
      phone: data.localGuardianInfo?.phone,
      email: data.localGuardianInfo?.email,
      address: data.localGuardianInfo?.address,
    });

    // Image preview (IMPORTANT)
    if (data.student?.imageUrl) {
      this.imagePreview = data.student.imageUrl;
    }
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
        fatherNationalIdNo: new FormControl(''),
        fatherMobile: new FormControl('', [Validators.required]),
        fatherTelephoneOffice: new FormControl(''),
        fatherTelephoneResidence: new FormControl(''),
        fatherEmail: new FormControl(''),
        motherName: new FormControl('', [Validators.required]),
        motherAcademicQualification: new FormControl(''),
        motherOccupation: new FormControl(''),
        motherNationalIdNo: new FormControl(''),
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

      // ✅ Guardian
      formData.append('FatherName', guardian.fatherName ?? '');
      formData.append('MotherName', guardian.motherName ?? '');

      // ✅ Local Guardian
      formData.append('LocalGuardianName', localGuardian.name ?? '');
      formData.append('LocalGuardianPhone', localGuardian.phone ?? '');

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
            'Failed to create sitemap. Please try again later.'
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
