import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { BillMasterService, ProcessBillRequest } from '../../services/bill-master.service';
import { AdmissionService } from '../../services/admission.service';
import { ButtonModule } from 'primeng/button';
import { DropdownModule } from 'primeng/dropdown';
import { InputNumberModule } from 'primeng/inputnumber';
import { ToastModule } from 'primeng/toast';
import { MessageService } from 'primeng/api';
import { EMPTY_GUID } from '../../../../core/constents';

@Component({
  selector: 'app-bill-process',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    ButtonModule,
    DropdownModule,
    InputNumberModule,
    ToastModule
  ],
  providers: [MessageService],
  templateUrl: './bill-process.component.html',
  styleUrl: './bill-process.component.scss'
})
export class BillProcessComponent implements OnInit {
  billProcessForm: FormGroup;
  admissions: any[] = [];
  isProcessing: boolean = false;

  months = [
    { label: 'January', value: 1 },
    { label: 'February', value: 2 },
    { label: 'March', value: 3 },
    { label: 'April', value: 4 },
    { label: 'May', value: 5 },
    { label: 'June', value: 6 },
    { label: 'July', value: 7 },
    { label: 'August', value: 8 },
    { label: 'September', value: 9 },
    { label: 'October', value: 10 },
    { label: 'November', value: 11 },
    { label: 'December', value: 12 }
  ];

  private fb = inject(FormBuilder);
  private billMasterService = inject(BillMasterService);
  private admissionService = inject(AdmissionService);
  private messageService = inject(MessageService);

  constructor() {
    const now = new Date();
    this.billProcessForm = this.fb.group({
      admissionId: [EMPTY_GUID, Validators.required],
      billMonth: [now.getMonth() + 1, Validators.required],
      billYear: [now.getFullYear(), Validators.required]
    });
  }

  ngOnInit() {
    this.loadAdmissions();
  }

  loadAdmissions() {
    this.admissionService.getAdmissionDropdown().subscribe(res => {
      if (res.isSuccess) this.admissions = res.data || [];
    });
  }

  processBill() {
    if (this.billProcessForm.invalid) {
      return;
    }

    this.isProcessing = true;
    const formValue = this.billProcessForm.value;

    const request: ProcessBillRequest = {
      admissionId: formValue.admissionId,
      billMonth: formValue.billMonth,
      billYear: formValue.billYear
    };

    this.billMasterService.processBill(request).subscribe({
      next: (res) => {
        this.isProcessing = false;
        if (res.isSuccess) {
          this.messageService.add({
            severity: 'success',
            summary: 'Successful',
            detail: 'Bill processed successfully',
            life: 3000
          });
        } else {
          this.messageService.add({
            severity: 'error',
            summary: 'Error',
            detail: res.errors?.join(', ') || 'Failed to process bill'
          });
        }
      },
      error: () => {
        this.isProcessing = false;
        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail: 'Failed to process bill'
        });
      }
    });
  }
}
