export const UserTypes1 = {
  SuperAdmin: { label: 'Super Admin', value: 1, id: 'SuperAdmin' },
  SystemUser: { label: 'System User', value: 3, id: 'SystemUser' },
  TenantAdmin: { label: 'Tenant Admin', value: 2, id: 'TenantAdmin' },
  TenantUser: { label: 'Tenant User', value: 4, id: 'TenantUser' },
};

export const MenuTypes = {
  Page: { label: 'Page', value: 1 },
  Tab: { label: 'Tab', value: 2 },
  Step: { label: 'Step', value: 3 },
  Feature: { label: 'Feature', value: 4 },
  ParentMenu: { label: 'ParentMenu', value: 5 },
};

export const UserTypes = {
  Internal: { label: 'Internal', value: 1 },
  External: { label: 'External', value: 2 },
  ThirdParty: { label: 'Third Party', value: 3 },
};

export const MenuAccessTypes = {
  SystemAdmin: { label: 'System Admin', value: 1 },
  TenantAdmin: { label: 'Tenant', value: 2 },
  Both: { label: 'Both', value: 3 },
};

export const ApprovalStatus = {
  Pending: { label: 'Pending', value: 1 },
  Approved: { label: 'Approved', value: 2 },
  Rejected: { label: 'Rejected', value: 3 },
  Cancelled: { label: 'Cancelled', value: 4 },
};

export enum Severity {
  SUCCESS = 'success',
  INFO = 'info',
  WARN = 'warn',
  ERROR = 'error',
}
