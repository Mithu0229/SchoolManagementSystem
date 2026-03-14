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
import { FormErrorComponent } from '../../../shared/components/form-error.component';
import { FormBase } from '../../../core/enums/form-base';
import { ToastService } from '../../../core/services/toast.service';
import { StudentService } from '../../../core/services/student.service';

@Component({
  selector: 'app-update-student-info',
  imports: [
    ReactiveFormsModule,
    CommonModule,
    InputTextModule,
    TextareaModule,
    SelectModule,
    ButtonModule,
    FormErrorComponent,
  ],
  templateUrl: './update-student-info.component.html',
  styleUrl: './update-student-info.component.scss',
})
export class UpdateStudentInfoComponent
  extends FormBase
  implements OnInit, OnDestroy
{
  applicationForm!: FormGroup;
  studentId!: string;
  selectedFile: File | null = null;
  imagePreview: string | ArrayBuffer | null = null;

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

  private readonly destroy$ = new Subject<void>();

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
    this.route.params.pipe(takeUntil(this.destroy$)).subscribe((params) => {
      if (params['id']) {
        this.studentId = params['id'];
        this.getStudentById(this.studentId);
      }
    });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  get form(): FormGroup {
    return this.applicationForm;
  }

  private getStudentById(id: string) {
    this.studentService
      .getStudentById(id)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (res) => {
          if (res && res.isSuccess && res.data) {
            const data = res.data;
            const dateValue = data?.dateOfBirth
              ? new Date(data.dateOfBirth).toISOString().split('T')[0]
              : null;

            this.applicationForm.patchValue({
              fullName: data?.fullName ?? '',
              gender: data?.gender ?? null,
              dateOfBirth: dateValue,
              placeOfBirth: data?.placeOfBirth ?? '',
              nationality: data?.nationality ?? '',
              religion: data?.religion ?? '',
              bloodGroup: data?.bloodGroup ?? '',
              birthCertificateNo: data?.birthCertificateNo ?? '',
              applicationForClass: data?.applicationForClass ?? '',
              academicYear: data?.academicYear ?? '',
              lastSchool: data?.lastSchool ?? '',
              lastClassAttendedResult: data?.lastClassAttendedResult ?? '',
              isDisability:
                (data?.isDisability ?? '').toString().toLowerCase() === 'true',
              disability: data?.disability ?? '',
              specialCare: data?.specialCare ?? '',
              presentAddress: data?.presentAddress ?? '',
              permanentAddress: data?.permanentAddress ?? '',
              studentPhone: data?.studentPhone ?? '',
              studentEmail: data?.studentEmail ?? '',
            });

            if (data?.imagePath) {
              this.imagePreview = 'http://localhost:5015' + data.imagePath;
              if (typeof this.imagePreview === 'string') {
                this.preloadSelectedFileFromImageUrl(this.imagePreview);
              }
            }

            this.syncImageControlValidation();
          } else {
            this.toastService.error(
              res?.notificationMessage || 'Failed to load student data.',
            );
          }
        },
        error: () => {
          this.toastService.error('Failed to load student data.');
        },
      });
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
    const imageControl = this.applicationForm.get('image');

    if (!imageControl) return;

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
    });

    this.syncImageControlValidation();
  }

  onSubmit() {
    this.markAllAsTouched();

    if (this.applicationForm.valid) {
      const formData = new FormData();
      const student = this.applicationForm.value;

      formData.append('Id', this.studentId);
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
      formData.append('LastSchool', student.lastSchool ?? '');
      formData.append(
        'LastClassAttendedResult',
        student.lastClassAttendedResult ?? '',
      );
      formData.append('StudentPhone', student.studentPhone ?? '');
      formData.append('StudentEmail', student.studentEmail ?? '');
      formData.append('IsDisability', student.isDisability ?? false);
      formData.append('Disability', student.disability ?? '');
      formData.append('SpecialCare', student.specialCare ?? '');

      if (this.selectedFile) {
        formData.append('Image', this.selectedFile);
      }

      this.studentService.updateStudentOnly(formData).subscribe({
        next: (res) => {
          if (res.isSuccess) {
            this.toastService.success(
              'Student info has been updated successfully.',
            );
            this.router.navigateByUrl('/application');
          } else {
            let errorMessage =
              'Failed to update student info. Please try again later.';
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
            'Failed to update student info. Please try again later.',
          );
        },
      });
    } else {
      this.applicationForm.markAllAsTouched();
    }
  }

  onCancel() {
    this.router.navigateByUrl('');
  }
}
