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
    { label: 'Male', value: 1 },
    { label: 'Female', value: 2 },
    { label: 'Other', value: 3 },
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
  selectedFile: File | null = null;
  imagePreview: string | ArrayBuffer | null = null;
  studentId!: string;
  guardianId: string | null = null;
  localGuardianId: string | null = null;

  constructor(
    private fb: FormBuilder,
    private studentService: StudentService,
    private toastService: ToastService,
    private readonly router: Router,
    private route: ActivatedRoute,
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

  getStudentById1(id: any) {
    this.studentService.getStudentById(id).subscribe({
      next: (response: any) => {
        if (response.isSuccess) {
          const roleData = response.data;
        }
      },
    });
  }

  getStudentById(id: string) {
    this.studentId = id;
    console.log('========== GET STUDENT BY ID ==========');
    console.log('Student ID:', id);
    console.log('Expected URL: http://localhost:5015/api/StudentInfo/' + id);
    console.log('=====================================');

    this.studentService
      .getStudentById(id)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (res) => {
          console.log('========== API RESPONSE SUCCESS ==========');
          console.log('Full Response:', res);
          console.log('Response Type:', typeof res);
          console.log('isSuccess:', res?.isSuccess);
          console.log('data:', res?.data);
          console.log('=====================================');

          if (res && res.isSuccess) {
            console.log('✅ Success - Patching form with data');
            this.imagePreview = 'http://localhost:5015' + res.data.imagePath;
            res.data.imagePath = this.imagePreview; // Add imageUrl to data for patching
            if (typeof this.imagePreview === 'string') {
              this.preloadSelectedFileFromImageUrl(this.imagePreview);
            }
            this.syncImageControlValidation();

            const dob = res.data?.dateOfBirth
              ? new Date(res.data.dateOfBirth).toISOString().split('T')[0]
              : null;
            res.data.dateOfBirth = dob;
            this.patchApplicationForm(res.data);
          } else {
            console.error('❌ API returned unsuccessful response');
            console.error('Response:', res);
            this.toastService.error(
              res?.notificationMessage || 'Failed to load student data.',
            );
          }
        },
        error: (err) => {
          let errorMessage = 'Failed to load student data. ';
          this.toastService.error(errorMessage);
        },
      });
  }

  patchApplicationForm(data: any) {
    if (!data) {
      console.warn('No data to patch');
      return;
    }
    console.log('Patching form with data:', data);

    const guardianInfo = data?.guardianInfo ?? data?.GuardianInfo ?? null;
    const localGuardianInfo =
      data?.localGuardianInfo ?? data?.LocalGuardianInfo ?? null;

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
      // image: data?.imagePath || null,
    });
    // Guardian
    this.guardianId = guardianInfo?.id ?? guardianInfo?.Id ?? null;
    this.applicationForm.get('guardian')?.patchValue({
      fatherName:
        guardianInfo?.fatherName ??
        guardianInfo?.FatherName ??
        data?.fatherName,
      fatherOccupation:
        guardianInfo?.fatherOccupation ?? guardianInfo?.FatherOccupation,
      fatherAcademicQualification:
        guardianInfo?.fatherAcademicQualification ??
        guardianInfo?.FatherAcademicQualification,
      fatherMobile: guardianInfo?.fatherMobile ?? guardianInfo?.FatherMobile,
      fatherEmail: guardianInfo?.fatherEmail ?? guardianInfo?.FatherEmail,
      fatherTelephoneOffice:
        guardianInfo?.fatherTelephoneOffice ??
        guardianInfo?.FatherTelephoneOffice,
      fatherTelephoneResidence:
        guardianInfo?.fatherTelephoneResidence ??
        guardianInfo?.FatherTelephoneResidence,
      fatherNationalIdNo:
        guardianInfo?.fatherNationalIdNo ?? guardianInfo?.FatherNationalIdNo,

      motherName:
        guardianInfo?.motherName ??
        guardianInfo?.MotherName ??
        data?.motherName,
      motherOccupation:
        guardianInfo?.motherOccupation ?? guardianInfo?.MotherOccupation,
      motherAcademicQualification:
        guardianInfo?.motherAcademicQualification ??
        guardianInfo?.MotherAcademicQualification,
      motherMobile: guardianInfo?.motherMobile ?? guardianInfo?.MotherMobile,
      motherEmail: guardianInfo?.motherEmail ?? guardianInfo?.MotherEmail,
      motherTelephoneOffice:
        guardianInfo?.motherTelephoneOffice ??
        guardianInfo?.MotherTelephoneOffice,
      motherTelephoneResidence:
        guardianInfo?.motherTelephoneResidence ??
        guardianInfo?.MotherTelephoneResidence,
      motherNationalIdNo:
        guardianInfo?.motherNationalIdNo ?? guardianInfo?.MotherNationalIdNo,
    });

    // Local Guardian
    this.localGuardianId =
      localGuardianInfo?.id ?? localGuardianInfo?.Id ?? null;
    this.applicationForm.get('localGuardian')?.patchValue({
      name: localGuardianInfo?.name ?? localGuardianInfo?.Name ?? data?.name,
      relationToStudent:
        localGuardianInfo?.relationToStudent ??
        localGuardianInfo?.RelationToStudent,
      phone: localGuardianInfo?.phone ?? localGuardianInfo?.Phone,
      email: localGuardianInfo?.email ?? localGuardianInfo?.Email,
      address: localGuardianInfo?.address ?? localGuardianInfo?.Address,
    });

    // Image preview - check both possible locations
    if (data?.imageUrl) {
      this.imagePreview = data.imageUrl;
      console.log('Image URL set from data.imageUrl:', data.imageUrl);
      this.preloadSelectedFileFromImageUrl(data.imageUrl);
    } else if (data?.student?.imageUrl) {
      this.imagePreview = data.student.imageUrl;
      console.log(
        'Image URL set from data.student.imageUrl:',
        data.student.imageUrl,
      );
      this.preloadSelectedFileFromImageUrl(data.student.imageUrl);
    }

    this.syncImageControlValidation();

    console.log('Form patched successfully');
  }

  onFileChange(event: Event) {
    const input = event.target as HTMLInputElement;

    if (!input.files || input.files.length === 0) return;

    this.selectedFile = input.files[0];
    this.syncImageControlValidation();

    const reader = new FileReader();
    reader.onload = () => {
      this.imagePreview = reader.result;
      this.syncImageControlValidation();
    };
    reader.readAsDataURL(this.selectedFile);
  }

  private syncImageControlValidation() {
    const imageControl = this.applicationForm.get('student.image');

    if (!imageControl) return;

    // In edit mode keep image required only when there is no existing image and no new upload.
    if (this.imagePreview || this.selectedFile) {
      imageControl.clearValidators();
    } else {
      imageControl.setValidators([Validators.required]);
    }

    imageControl.updateValueAndValidity({ emitEvent: false });
  }

  private preloadSelectedFileFromImageUrl(imageUrl: string) {
    if (!imageUrl || imageUrl.startsWith('data:') || this.selectedFile) {
      return;
    }

    fetch(imageUrl)
      .then((response) => {
        if (!response.ok) {
          throw new Error('Failed to fetch existing image');
        }
        return response.blob();
      })
      .then((blob) => {
        const cleanUrl = imageUrl.split('?')[0];
        const fallbackName =
          cleanUrl.substring(cleanUrl.lastIndexOf('/') + 1) ||
          'existing-image.jpg';
        const hasExtension = fallbackName.includes('.');
        const extension = blob.type?.split('/')[1] || 'jpg';
        const fileName = hasExtension
          ? fallbackName
          : `${fallbackName}.${extension}`;

        this.selectedFile = new File([blob], fileName, {
          type: blob.type || 'image/jpeg',
        });
        this.syncImageControlValidation();
      })
      .catch(() => {
        // Keep preview-only mode if image cannot be fetched as a File.
      });
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

    this.syncImageControlValidation();
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
      if (this.guardianId) {
        formData.append('GuardianInfo.Id', this.guardianId);
      }
      formData.append('GuardianInfo.StudentInfoId', this.studentId);
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
      formData.append(
        'GuardianInfo.FatherNationalIdNo',
        guardian.fatherNationalIdNo ?? '',
      );

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
      formData.append(
        'GuardianInfo.MotherNationalIdNo',
        guardian.motherNationalIdNo ?? '',
      );

      // ✅ Local Guardian (nested binding for [FromForm] StudentInfoRequest)
      if (this.localGuardianId) {
        formData.append('LocalGuardianInfo.Id', this.localGuardianId);
      }
      formData.append('LocalGuardianInfo.StudentInfoId', this.studentId);
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
      formData.append('Id', this.studentId);

      this.studentService.updateStudent(formData).subscribe({
        next: (res) => {
          if (res.isSuccess) {
            this.toastService.success('Student has been updated successfully.');
            this.router.navigateByUrl('/application');
          } else {
            let errorMessage =
              'Failed to update student. Please try again later.';
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
            'Failed to update student. Please try again later.',
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
