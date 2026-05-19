import { Component, OnInit, inject, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators, FormArray } from '@angular/forms';
import { FeeCollection, FeeCollectionService } from '../../services/fee-collection.service';
import { TableComponent } from '../../../../shared/components/table/table.component';
import { TableColumn, TableConfig } from '../../../../shared/components/table/table.interface';
import { StudentService } from '../../services/student.service';
import { AdmissionService } from '../../services/admission.service';
import { BranchService } from '../../services/branch.service';
import { FinancialYearService } from '../../services/financial-year.service';
import { FeeHeadService } from '../../services/fee-head.service';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { InputTextModule } from 'primeng/inputtext';
import { InputNumberModule } from 'primeng/inputnumber';
import { CalendarModule } from 'primeng/calendar';
import { CheckboxModule } from 'primeng/checkbox';
import { DropdownModule } from 'primeng/dropdown';
import { ToastModule } from 'primeng/toast';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { MessageService, ConfirmationService } from 'primeng/api';
import { EMPTY_GUID } from '../../../../core/constents';

@Component({
  selector: 'app-fee-collection',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    TableComponent,
    ButtonModule,
    DialogModule,
    InputTextModule,
    InputNumberModule,
    CalendarModule,
    CheckboxModule,
    DropdownModule,
    ToastModule,
    ConfirmDialogModule
  ],
  providers: [MessageService, ConfirmationService],
  templateUrl: './fee-collection.component.html',
  styleUrl: './fee-collection.component.scss'
})
export class FeeCollectionComponent implements OnInit {
  @ViewChild(TableComponent) tableComponent!: TableComponent;

  feeCollections: FeeCollection[] = [];
  feeCollectionDialog: boolean = false;
  feeCollectionForm: FormGroup;
  isEditMode: boolean = false;
  submitted: boolean = false;

  students: any[] = [];
  admissions: any[] = [];
  branches: any[] = [];
  financialYears: any[] = [];
  feeHeads: any[] = [];

  paymentModes = [
    { label: 'Cash', value: 'Cash' },
    { label: 'Bank', value: 'Bank' },
    { label: 'Mobile Banking', value: 'Mobile Banking' }
  ];

  columns: TableColumn[] = [];
  tableConfig!: TableConfig;

  private fb = inject(FormBuilder);
  private feeCollectionService = inject(FeeCollectionService);
  private studentService = inject(StudentService);
  private admissionService = inject(AdmissionService);
  private branchService = inject(BranchService);
  private financialYearService = inject(FinancialYearService);
  private feeHeadService = inject(FeeHeadService);
  private messageService = inject(MessageService);
  private confirmationService = inject(ConfirmationService);

  constructor() {
    this.feeCollectionForm = this.fb.group({
      id: [''],
      collectionDate: [new Date(), Validators.required],
      memoNo: [''],
      studentId: [EMPTY_GUID, Validators.required],
      admissionId: [EMPTY_GUID, Validators.required],
      branchId: [EMPTY_GUID, Validators.required],
      financialYearId: [EMPTY_GUID, Validators.required],
      totalAmount: [0, Validators.required],
      discountAmount: [0],
      paidAmount: [0, Validators.required],
      dueAmount: [0],
      paymentMode: ['Cash'],
      remarks: [''],
      isCancelled: [false],
      isActive: [true],
      details: this.fb.array([])
    });
  }

  get details() {
    return this.feeCollectionForm.get('details') as FormArray;
  }

  addDetail() {
    const detailForm = this.fb.group({
      id: [EMPTY_GUID],
      feeCollectionId: [EMPTY_GUID, Validators.required],
      feeHeadId: [EMPTY_GUID, Validators.required],
      monthNo: ['', Validators.required],
      yearNo: ['', Validators.required],
      feeAmount: [0, Validators.required],
      discountAmount: [0],
      paidAmount: [0, Validators.required],
      dueAmount: [0]
    });
    this.details.push(detailForm);
  }

  removeDetail(index: number) {
    this.details.removeAt(index);
  }

  ngOnInit() {
    this.initializeColumns();
    this.initializeTableConfig();
    this.loadDropdowns();
  }

  loadDropdowns() {
    this.studentService.getStudentDropdown().subscribe(res => {
      if (res.isSuccess) this.students = res.data || [];
    });
    this.admissionService.getAdmissionDropdown().subscribe(res => {
      if (res.isSuccess) this.admissions = res.data || [];
    });
    this.branchService.getBranchDropdown().subscribe(res => {
      if (res.isSuccess) this.branches = res.data || [];
    });
    this.financialYearService.getFinancialYearDropdown().subscribe(res => {
      if (res.isSuccess) this.financialYears = res.data || [];
    });
    this.feeHeadService.getFeeHeadDropdown().subscribe(res => {
      if (res.isSuccess) this.feeHeads = res.data || [];
    });
  }

  initializeColumns(): void {
    this.columns = [
      { field: 'collectionDate', header: 'Date', sortable: true, dataType: 'date' },
      { field: 'memoNo', header: 'Memo No', sortable: true },
      { field: 'paidAmount', header: 'Paid', sortable: true },
      { field: 'isActive', header: 'Status', sortable: true, dataType: 'boolean' }
    ];

    this.columns.push({
      isActionColumn: true,
      field: 'Actions',
      header: 'Actions',
      actions: [
        {
          label: 'Edit',
          icon: 'pi pi-pencil',
          callback: (row) => this.editFeeCollection(row),
          visible: () => true,
        },
        {
          label: 'Delete',
          icon: 'pi pi-trash',
          styleClass: 'p-button-danger',
          callback: (row) => this.deleteFeeCollection(row),
          visible: () => true,
        }
      ],
    });
  }

  initializeTableConfig(): void {
    this.tableConfig = {
      pageSize: 10,
      pageSizeOptions: [5, 10, 25],
      showSearch: true,
      searchPlaceholder: 'Search here',
      emptyMessage: 'No collections found',
      showCreateButton: true,
      showCheckboxColumn: false,
      createButtonLabel: 'Add Collection',
    };
  }

  openNew() {
    this.feeCollectionForm.reset({
      collectionDate: new Date(),
      studentId: EMPTY_GUID,
      admissionId: EMPTY_GUID,
      branchId: EMPTY_GUID,
      financialYearId: EMPTY_GUID,
      totalAmount: 0,
      discountAmount: 0,
      paidAmount: 0,
      dueAmount: 0,
      paymentMode: 'Cash',
      isCancelled: false,
      isActive: true
    });
    while (this.details.length) {
      this.details.removeAt(0);
    }
    this.isEditMode = false;
    this.submitted = false;
    this.feeCollectionDialog = true;
  }

  editFeeCollection(feeCollection: FeeCollection) {
    this.isEditMode = true;
    this.submitted = false;

    this.feeCollectionForm.patchValue({
      ...feeCollection,
      collectionDate: feeCollection.collectionDate ? new Date(feeCollection.collectionDate) : null
    });

    while (this.details.length) {
      this.details.removeAt(0);
    }

    feeCollection.details?.forEach(detail => {
      this.details.push(this.fb.group({
        ...detail
      }));
    });

    this.feeCollectionDialog = true;
  }

  deleteFeeCollection(feeCollection: FeeCollection) {
    this.confirmationService.confirm({
      message: 'Are you sure you want to delete this collection entry?',
      header: 'Confirm',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        if (feeCollection.id) {
          this.feeCollectionService.deleteFeeCollection(feeCollection.id).subscribe({
            next: () => {
              this.messageService.add({ severity: 'success', summary: 'Successful', detail: 'Collection Deleted', life: 3000 });
              this.tableComponent.loadData();
            },
            error: () => {
              this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Failed to delete collection' });
            }
          });
        }
      }
    });
  }

  hideDialog() {
    this.feeCollectionDialog = false;
    this.submitted = false;
  }

  saveFeeCollection() {
    this.submitted = true;

    if (this.feeCollectionForm.invalid) {
      return;
    }

    const formValue = this.feeCollectionForm.value;
    const payload: FeeCollection = {
      ...formValue,
      id: formValue.id || EMPTY_GUID
    };

    if (this.isEditMode) {
      this.feeCollectionService.updateFeeCollection(payload).subscribe({
        next: (res) => {
          this.messageService.add({ severity: 'success', summary: 'Successful', detail: 'Collection Updated', life: 3000 });
          this.tableComponent.loadData();
          this.hideDialog();
        },
        error: () => {
          this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Failed to update collection' });
        }
      });
    } else {
      this.feeCollectionService.createFeeCollection(payload).subscribe({
        next: (res) => {
          this.messageService.add({ severity: 'success', summary: 'Successful', detail: 'Collection Created', life: 3000 });
          this.tableComponent.loadData();
          this.hideDialog();
        },
        error: () => {
          this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Failed to create collection' });
        }
      });
    }
  }
}
