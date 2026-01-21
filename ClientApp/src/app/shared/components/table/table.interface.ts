export interface TableColumn {
  field: string;
  header: string;
  width?: string;
  sortable?: boolean;
  filterable?: boolean;
  dataType?:
    | 'text'
    | 'number'
    | 'date'
    | 'datetime'
    | 'boolean'
    | 'long-text'
    | 'time'
    | 'long-text-with-popover'
    | 'url';
  isActionColumn?: boolean;
  actions?: TableAction[];
  trueLabel?: string;
  falseLabel?: string;
  popOverField?: string;
  customTemplate?: boolean;
}

export interface TableConfig {
  pageSize?: number;
  pageSizeOptions?: number[];
  defaultSortField?: string;
  defaultSortOrder?: 'asc' | 'desc';
  showSearch?: boolean;
  searchPlaceholder?: string;
  loadingMessage?: string;
  emptyMessage?: string;
  showCreateButton?: boolean;
  createButtonLabel?: string;
  createButtonPermission?: boolean;
  createButtonIcon?: string;
  customFilters?: CustomFilter[];
  showExportButton?: boolean;
  showExportPdfButton?: boolean;
  showImportButton?: boolean;
  isFilterByDate?: boolean;
  hideSerial?: boolean;
  showPrintButton?: boolean;
  columnVisibility?: Record<string, boolean>;
  showCheckboxColumn?: boolean;
  selectableRows?: boolean;
  preselectedRows?: any[]; // Array of IDs or objects to pre-select
  rowIdentifier?: string; // Field name that uniquely identifies a row (default: 'id')
}

export interface CustomFilter {
  field: string;
  label: string;
  options: { label: string; value: any }[];
  placeholder?: string;
}

export interface TableState {
  page: number;
  first: number;
  pageSize: number;
  sortField: string | string[];
  sortOrder?: 'asc' | 'desc';
  searchQuery?: string;
  customFilters?: Record<string, any>;
  selectedRows: any[]; // Track selected rows
  selectedRowIds: Set<string | number>; // Track selected IDs for efficiency
}

export interface TableAction<T = any> {
  label?: string;
  icon?: string;
  styleClass?: string;
  callback: (rowData: T) => void;
  disabled?: (rowData: T) => boolean;
  visible?: (rowData: T) => boolean;
}

export interface FilterItem {
  field: string;
  value: any;
}

export interface TableAPIPayload {
  page: number;
  pageSize: number;
  sortColumn: string | null;
  sortDirection: 'asc' | 'desc' | null | '';
  search: string;
  filters?: FilterItem[];
  startDate?: string;
  endDate?: string;
}
