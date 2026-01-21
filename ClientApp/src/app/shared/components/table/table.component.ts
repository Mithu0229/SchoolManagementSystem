import {
  Component,
  Input,
  Output,
  EventEmitter,
  ViewChild,
  ElementRef,
  signal,
  OnInit,
  OnChanges,
  SimpleChanges,
} from '@angular/core';
import { Table, TableLazyLoadEvent, TableModule } from 'primeng/table';
import {
  TableAPIPayload,
  TableColumn,
  TableConfig,
  TableState,
} from './table.interface';
import { CommonModule } from '@angular/common';
import { SkeletonModule } from 'primeng/skeleton';
import { InputTextModule } from 'primeng/inputtext';
import { ButtonModule } from 'primeng/button';
import { TooltipModule } from 'primeng/tooltip';
import { ConfirmDialogConfig } from '../confirm-dialog/confirm-dialog.interface';
import { FormatDatePipe } from '../../../core/pipes/format-date.pipe';
import { CapitalizePipe } from '../../../core/pipes/capitalize.pipe';
import { TagModule } from 'primeng/tag';
import { PopoverModule } from 'primeng/popover';
import { DropdownModule } from 'primeng/dropdown';
import { FormsModule } from '@angular/forms';
import { FileUploadModule } from 'primeng/fileupload';
import { EmptyStateComponent } from './empty-state.component';
import { DatePickerModule } from 'primeng/datepicker';
import { CheckboxModule } from 'primeng/checkbox';
import { MultiSelectModule } from 'primeng/multiselect';
import dayjs from 'dayjs';
import utc from 'dayjs/plugin/utc';
import { TableService } from '../../../core/services/table.service';
import { ToastService } from '../../../core/services/toast.service';
import { DisableAutofillDirective } from '../../directives/disable-autofill.directive';
import { debounceTime, Subject, switchMap } from 'rxjs';
import { TableDataService } from './table-data.service';

dayjs.extend(utc);

@Component({
  selector: 'app-table',
  imports: [
    TableModule,
    CommonModule,
    SkeletonModule,
    InputTextModule,
    ButtonModule,
    TooltipModule,
    FormatDatePipe,
    TagModule,
    PopoverModule,
    DropdownModule,
    FormsModule,
    FileUploadModule,
    EmptyStateComponent,
    DatePickerModule,
    CheckboxModule,
    MultiSelectModule,
    DisableAutofillDirective,
  ],
  standalone: true,
  templateUrl: './table.component.html',
  styleUrl: './table.component.scss',
  providers: [CapitalizePipe],
})
export class TableComponent implements OnInit, OnChanges {
  @ViewChild('dt') table!: Table;
  @ViewChild('searchInput') searchInput!: ElementRef<HTMLInputElement>;
  startDate!: Date;
  endDate!: Date;

  @Input() apiUrl!: string;
  @Input() columns: TableColumn[] = [];
  @Input() config: TableConfig = {};
  @Input() preselectedRows: any[] = [];

  @Output() actionClicked = new EventEmitter();
  @Output() importHandler = new EventEmitter();
  @Output() exportCSVHandler = new EventEmitter();
  @Output() exportPDFHandler = new EventEmitter();
  @Output() printHandler = new EventEmitter();
  @Output() dateRangeChange = new EventEmitter<{
    startDate: Date | null;
    endDate: Date | null;
  }>();
  @Output() selectedRowsChange = new EventEmitter<any[]>();

  @Input() dialogConfig: ConfirmDialogConfig = {
    visible: false,
    title: 'Delete Confirmation',
    message: 'Are you sure you want to delete this item?',
    type: 'delete',
    acceptLabel: 'Delete',
    rejectLabel: 'Cancel',
  };
  private loadTrigger$ = new Subject<boolean>();
  loading = signal(false);
  data: any[] = [];
  totalRecords = 0;
  private searchDebounce: any;
  columnVisibility: Record<string, boolean> = {};
  showColumnOptionsDropdown = false;
  selectedColumns: TableColumn[] = [];

  currentState: TableState = {
    page: 0,
    first: 0,
    pageSize: this.config.pageSize ?? 10,
    sortField: this.config.defaultSortField ?? '',
    sortOrder: this.config.defaultSortOrder,
    customFilters: {},
    selectedRows: [],
    selectedRowIds: new Set(),
  };
  constructor(
    private readonly capitalize: CapitalizePipe,
    private readonly toastService: ToastService,
    private tableDataService: TableDataService
  ) {}

  ngOnInit(): void {
    this.initializeColumnVisibility();
    // ✅ Setup reactive loader
    this.loadTrigger$
      .pipe(
        debounceTime(200), // prevent multiple rapid calls
        switchMap((forceRefresh) => {
          this.loading.set(true);
          return this.tableDataService.getTableData(
            this.apiUrl,
            this.currentState,
            this.columns,
            this.capitalize,
            forceRefresh
          );
        })
      )
      .subscribe({
        next: (res) => {
          this.data = res.items;
          this.totalRecords = res.totalRecord;
          this.loading.set(false);
        },
        error: () => {
          this.loading.set(false);
        },
      });
    if (this.preselectedRows && this.preselectedRows.length > 0) {
      this.setPreselectedRows();
    }
  }

  ngOnChanges(changes: SimpleChanges): void {
    console.log('changes : ', changes);
    if (changes['columns'] && changes['columns'].currentValue) {
      this.initializeColumnVisibility();
    }
    if (changes['config'] && changes['config'].currentValue?.preselectedRows) {
      this.setPreselectedRows();
    }
  }

  private setPreselectedRows() {
    const identifier = this.config.rowIdentifier || 'id';
    this.preselectedRows?.forEach((row) => {
      const id = typeof row === 'object' ? row[identifier] : row;
      this.currentState.selectedRowIds.add(id);
    });
    this.currentState.selectedRows = [...(this.preselectedRows || [])];
    this.selectedRowsChange.emit(this.currentState.selectedRows);
  }

  private initializeColumnVisibility() {
    // Clear existing column visibility
    this.columnVisibility = {};

    // Initialize all columns as visible by default and set selectedColumns
    const nonActionColumns = this.columns.filter((col) => !col.isActionColumn);
    this.selectedColumns = [...nonActionColumns];

    this.columns.forEach((column) => {
      if (!column.isActionColumn) {
        this.columnVisibility[column.field] =
          this.config.columnVisibility?.[column.field] ?? true;
      }
    });
  }

  onRowSelect(event: any, rowData: any) {
    const identifier = this.config.rowIdentifier || 'id';
    const rowId = rowData[identifier];
    if (event.checked) {
      this.currentState.selectedRowIds.add(rowId);
      this.currentState.selectedRows.push(rowData);
    } else {
      this.currentState.selectedRowIds.delete(rowId);
      this.currentState.selectedRows = this.currentState.selectedRows.filter(
        (row) => row[identifier] !== rowId
      );
    }

    this.selectedRowsChange.emit(this.currentState.selectedRows);
  }

  isRowSelected(rowData: any): boolean {
    if (!rowData) return false;
    const identifier = this.config.rowIdentifier || 'id';
    const rowId = rowData[identifier];
    return this.currentState.selectedRowIds.has(rowId);
  }

  isColumnVisible(field: string): boolean {
    return this.columnVisibility[field] !== false;
  }

  get visibleColumns(): TableColumn[] {
    const nonActionColumns = this.columns.filter(
      (col) => !col.isActionColumn && this.isColumnVisible(col.field)
    );
    const actionColumns = this.columns.filter((col) => col.isActionColumn);

    // Hide action columns if no regular columns are visible
    if (nonActionColumns.length === 0) {
      return [];
    }

    return [...nonActionColumns, ...actionColumns];
  }

  get columnsForOptions(): TableColumn[] {
    return this.columns.filter((col) => !col.isActionColumn);
  }

  get isFiltered(): boolean {
    const hasSearchQuery = !!(
      this.currentState.searchQuery && this.currentState.searchQuery.length > 0
    );
    const hasCustomFilters = !!(
      this.currentState.customFilters &&
      Object.values(this.currentState.customFilters).some(
        (value) => value !== null && value !== undefined
      )
    );
    return hasSearchQuery || hasCustomFilters;
  }

  onLazyLoad(event: TableLazyLoadEvent) {
    this.currentState.page = Math.floor(
      (event.first ?? 0) / (event.rows ?? 10)
    );
    this.currentState.pageSize = event.rows ?? 10;
    this.currentState.sortField = event.sortField ?? '';
    this.currentState.sortOrder = event.sortOrder === 1 ? 'asc' : 'desc';
    this.currentState.first = event.first ?? 1;
    this.loadData();
  }

  // loadData() {
  //     this.loading.set(true);
  //     this.data = [];
  //     let params: TableAPIPayload = {
  //         page: this.currentState.page,
  //         pageSize: this.currentState.pageSize,
  //         sortColumn: '',
  //         sortDirection: '',
  //         search: this.currentState.searchQuery ?? '',
  //         filters: []
  //     };

  //     // Add custom filters if present
  //     if (this.currentState.customFilters && Object.keys(this.currentState.customFilters).length > 0) {
  //         for (const [field, value] of Object.entries(this.currentState.customFilters)) {
  //             if (value !== null && value !== undefined) {
  //                 params.filters?.push({
  //                     field: field,
  //                     value: value
  //                 });
  //             }
  //         }
  //     }

  //     if (this.currentState.sortField) {
  //         let sortField = '';
  //         if (this.currentState?.sortField) sortField = this.capitalize.transform(this.currentState.sortField.toString() ?? '');
  //         if (sortField === 'Serial') sortField = this.capitalize.transform(this.columns[0].field);
  //         params.sortColumn = sortField ?? '';
  //         params.sortDirection = this.currentState.sortOrder ?? '';
  //     }

  //     if (this.startDate && this.endDate) {
  //         params.startDate = dayjs(this.startDate).format('YYYY-MM-DD');
  //         params.endDate = dayjs(this.endDate).format('YYYY-MM-DD');
  //     }
  //     try {
  //         this.tableService.getTableData(this.apiUrl, params).subscribe({
  //             next: (response) => {
  //                 const parsedData = response.data;
  //                 this.data = parsedData?.items ?? [];
  //                 this.totalRecords = parsedData?.totalRecord ?? 0;
  //                 this.loading.set(false);
  //             },
  //             error: () => {
  //                 this.loading.set(false);
  //             }
  //         });
  //     } catch (e) {
  //         console.log('Error in Table : ', e);
  //         this.loading.set(false);
  //     }
  // }

  loadData(forceRefresh = true): void {
    this.loading.set(true);
    this.data = [];
    this.loadTrigger$.next(forceRefresh);
    // this.tableDataService.getTableData(this.apiUrl, this.currentState, this.columns, this.capitalize, forceRefresh).subscribe({
    //     next: (res) => {
    //         this.data = res.items;
    //         this.totalRecords = res.totalRecord;
    //         this.loading.set(false);
    //     },
    //     error: () => {
    //         this.loading.set(false);
    //     }
    // });
  }

  onSearch(searchQuery: string) {
    clearTimeout(this.searchDebounce);
    this.searchDebounce = setTimeout(() => {
      this.currentState.searchQuery = searchQuery;
      this.currentState.page = 0; // Reset to first page when searching
      this.loadData();
    }, 300);
  }

  onCustomFilterChange(field: string, value: any) {
    this.currentState.customFilters ??= {};
    this.currentState.customFilters[field] = value;
    this.currentState.page = 0; // Reset to first page when filtering
    this.loadData();
  }

  onDateFilterChange() {
    this.currentState.customFilters ??= {};

    if (this.startDate && this.endDate) {
      if (this.endDate < this.startDate) {
        this.toastService.error(
          'End date must be greater than or equal start date',
          'Invalid Date Range'
        );
        return;
      }
    }

    this.dateRangeChange.emit({
      startDate: this.startDate || null,
      endDate: this.endDate || null,
    });

    if (!this.startDate || !this.endDate) return;

    this.currentState.page = 0; // Reset to first page when filtering
    this.loadData();
  }

  resetFilters() {
    // Clear search query
    this.currentState.searchQuery = '';
    if (this.searchInput) {
      this.searchInput.nativeElement.value = '';
    }

    // Clear custom filters
    this.currentState.customFilters = {};

    // Reset pagination and reload data
    this.currentState.page = 0;
    this.table.first = 0;
    this.loadData();
  }
  onColumnSelectionChange(selectedCols: TableColumn[]) {
    this.selectedColumns = selectedCols;

    // Update column visibility based on selection
    const nonActionColumns = this.columns.filter((col) => !col.isActionColumn);
    nonActionColumns.forEach((col) => {
      this.columnVisibility[col.field] = selectedCols.some(
        (selected) => selected.field === col.field
      );
    });
  }

  toggleAllRows(checked: boolean) {
    const identifier = this.config.rowIdentifier || 'id';

    if (checked) {
      // Select all visible rows
      this.data.forEach((row) => {
        const rowId = row[identifier];
        if (!this.currentState.selectedRowIds.has(rowId)) {
          this.currentState.selectedRowIds.add(rowId);
          this.currentState.selectedRows.push(row);
        }
      });
    } else {
      // Deselect all visible rows
      this.data.forEach((row) => {
        const rowId = row[identifier];
        this.currentState.selectedRowIds.delete(rowId);
      });
      // Filter out any rows that are in the current page
      this.currentState.selectedRows = this.currentState.selectedRows.filter(
        (row) => !this.data.some((r) => r[identifier] === row[identifier])
      );
    }

    this.selectedRowsChange.emit(this.currentState.selectedRows);
  }

  areAllRowsSelected(): boolean {
    if (this.data.length === 0) return false;
    return this.data.every((row) => this.isRowSelected(row));
  }

  ngOnDestroy() {
    clearTimeout(this.searchDebounce);
  }
}
